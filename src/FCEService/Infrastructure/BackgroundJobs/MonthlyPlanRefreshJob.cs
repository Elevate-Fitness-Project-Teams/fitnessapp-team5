using FCEService.Application.Common.Interfaces;
using FCEService.Application.Common.Services;
using FCEService.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
/// PRODUCTION NOTE:
///   In a multi-pod (Kubernetes) environment, this job will run on every instance.
///   Add a Distributed Lock (e.g., Redis) before processing to prevent duplicate runs.
/// </summary>
public sealed class MonthlyPlanRefreshJob(
    IServiceScopeFactory scopeFactory,
    ILogger<MonthlyPlanRefreshJob> logger
) : BackgroundService
{
    private static readonly TimeSpan CheckInterval = TimeSpan.FromHours(24);
    private const int PlanExpiryDays = 30;

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

            // Wait 24 hours before next check
            await Task.Delay(CheckInterval, stoppingToken);
        }

        logger.LogInformation("MonthlyPlanRefreshJob stopped.");
    }

    private async Task ProcessExpiredPlansAsync(CancellationToken ct)
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

        foreach (var item in expiredUsers)
        {
            try
            {
                var bmr = FitnessCalculator.CalculateBmr(
                    item.stats.Weight, item.stats.Height, item.stats.Age, item.stats.Gender);

                var tdeeResult = FitnessCalculator.CalculateTdee(bmr, item.stats.ActivityLevel);
                if (tdeeResult.IsError) continue;

                var calorieTargetResult = FitnessCalculator.CalculateCalorieTarget(tdeeResult.Value, item.stats.Goal);
                if (calorieTargetResult.IsError) continue;

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
                    continue;
                }

                // Commit per user — one failure doesn't block others
                await db.SaveChangesAsync(ct);

                logger.LogInformation("MonthlyPlanRefreshJob: Plan refreshed for UserId: {UserId}", item.plan.UserId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "MonthlyPlanRefreshJob: Failed to refresh plan for UserId: {UserId}", item.plan.UserId);
            }
        }
    }
}
