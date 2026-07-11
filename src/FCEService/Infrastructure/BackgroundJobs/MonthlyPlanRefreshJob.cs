using FCEService.Application.Common.Interfaces;
using FCEService.Application.Common.Services;
using FCEService.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FCEService.Infrastructure.BackgroundJobs;

/// <summary>
/// Runs every 24 hours and reassigns fitness plans for users whose plan
/// has been active for more than 30 days without any change.
///
/// WHY:
///   Fitness plans should be refreshed monthly to reflect natural changes in
///   the user's body and fitness level, even if they didn't explicitly request it.
///
/// DISTRIBUTED LOCK:
///   Uses Redis SET NX (via IDistributedCache) to ensure only ONE pod runs this
///   job at a time in a multi-replica Kubernetes environment.
/// </summary>
public sealed class MonthlyPlanRefreshJob(
    IServiceScopeFactory scopeFactory,
    IConnectionMultiplexer redis,
    ILogger<MonthlyPlanRefreshJob> logger
) : BackgroundService
{
    private static readonly TimeSpan CheckInterval = TimeSpan.FromHours(24);
    private const int PlanExpiryDays = 30;
    private const string LockKey = "FCEService:MonthlyPlanRefreshJob:Lock";

    // Lock TTL is slightly less than CheckInterval so the next run can always acquire
    private static readonly TimeSpan LockExpiry = TimeSpan.FromHours(23);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("MonthlyPlanRefreshJob started.");

        // Wait 1 minute on startup to let the application fully initialize
        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("MonthlyPlanRefreshJob running at: {Time}", DateTimeOffset.UtcNow);

            try
            {
                await ProcessExpiredPlansAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "MonthlyPlanRefreshJob encountered an error.");
            }

            await Task.Delay(CheckInterval, stoppingToken);
        }

        logger.LogInformation("MonthlyPlanRefreshJob stopped.");
    }

    private async Task ProcessExpiredPlansAsync(CancellationToken ct)
    {
        // Atomic distributed lock via SET NX PX (Redis native, single-operation).
        // This avoids the TOCTOU race of GET+SET (two separate operations).
        // StringSetAsync(When.NotExists) maps directly to: SET key value PX ms NX
        var acquired = await TryAcquireLockAsync(ct);
        if (!acquired)
        {
            logger.LogInformation("MonthlyPlanRefreshJob: lock already held by another pod. Skipping this run.");
            return;
        }

        try
        {
            using var scope = scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
            var planAssignmentService = scope.ServiceProvider.GetRequiredService<IPlanAssignmentService>();

            var expiryThreshold = DateTimeOffset.UtcNow.AddDays(-PlanExpiryDays);

            // Single query: JOIN expired plans with stats and metrics.
            // Eliminates N+1 problem — was (N * 2) queries, now is 1 query.
            var expiredUsers = await db.UserAssignedPlans
                .Where(p => p.IsActive && p.CreatedAtUtc < expiryThreshold)
                .Join(db.UserFitnessStats,
                    plan => plan.UserId,
                    stats => stats.UserId,
                    (plan, stats) => new { plan, stats })
                .Join(db.CalculatedMetrics,
                    x => x.plan.UserId,
                    metrics => metrics.UserId,
                    (x, metrics) => new { x.plan, x.stats, metrics })
                .ToListAsync(ct);

            if (expiredUsers.Count == 0)
            {
                logger.LogInformation("MonthlyPlanRefreshJob: No expired plans found.");
                return;
            }

            logger.LogInformation("MonthlyPlanRefreshJob: Found {Count} expired plans to refresh.", expiredUsers.Count);

            int refreshed = 0;
            int failed = 0;

            foreach (var item in expiredUsers)
            {
                try
                {
                    var bmr = FitnessCalculator.CalculateBmr(
                        item.stats.Weight, item.stats.Height, item.stats.Age, item.stats.Gender);

                    var tdeeResult = FitnessCalculator.CalculateTdee(bmr, item.stats.ActivityLevel);
                    if (tdeeResult.IsError) { failed++; continue; }

                    var calorieTargetResult = FitnessCalculator.CalculateCalorieTarget(tdeeResult.Value, item.stats.Goal);
                    if (calorieTargetResult.IsError) { failed++; continue; }

                    var status = FitnessCalculator.DetermineStatus(calorieTargetResult.Value);

                    var result = await planAssignmentService.AssignPlanAsync(
                        item.plan.UserId,
                        item.stats.Goal,
                        status,
                        "Monthly auto-refresh",
                        ct);

                    if (result.IsError)
                    {
                        logger.LogWarning("MonthlyPlanRefreshJob: Could not reassign plan for UserId: {UserId}. Error: {Error}",
                            item.plan.UserId, result.Errors[0].Description);
                        failed++;
                        continue;
                    }

                    refreshed++;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "MonthlyPlanRefreshJob: Failed to refresh plan for UserId: {UserId}", item.plan.UserId);
                    failed++;
                }
            }

            // Single SaveChanges — commits ALL plan reassignments in one DB round trip
            // instead of N separate transactions inside the loop
            await db.SaveChangesAsync(ct);

            logger.LogInformation(
                "MonthlyPlanRefreshJob complete. Refreshed: {Refreshed}, Failed: {Failed}",
                refreshed, failed);
        }
        finally
        {
            // Always release the lock so the next scheduled run can proceed
            await ReleaseLockAsync(ct);
        }
    }

    private async Task<bool> TryAcquireLockAsync(CancellationToken ct)
    {
        try
        {
            // SET NX PX — atomic: only sets if key does NOT already exist.
            // This is ONE Redis operation, not two — no TOCTOU race condition.
            // When.NotExists maps to the NX flag in: SET key value PX milliseconds NX
            var db = redis.GetDatabase();
            return await db.StringSetAsync(
                LockKey,
                Environment.MachineName,
                LockExpiry,
                When.NotExists);
        }
        catch (Exception)
        {
            // If Redis is unavailable, run anyway (better than skipping the monthly refresh)
            logger.LogWarning("MonthlyPlanRefreshJob: Redis unavailable for lock. Running without distributed lock.");
            return true;
        }
    }

    private async Task ReleaseLockAsync(CancellationToken ct)
    {
        try
        {
            var db = redis.GetDatabase();
            await db.KeyDeleteAsync(LockKey);
        }
        catch { /* best-effort: lock will auto-expire via TTL */ }
    }
}
