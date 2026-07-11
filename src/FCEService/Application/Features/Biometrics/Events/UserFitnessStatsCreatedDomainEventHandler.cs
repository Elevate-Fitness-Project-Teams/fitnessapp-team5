using FCEService.Application.Common.Interfaces;
using FCEService.Domain.Entities.CalculatedMetrics;
using FCEService.Domain.Entities.UserFitnessStats;
using FCEService.Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FCEService.Application.Features.Biometrics.Events;

/// <summary>
/// Reacts to UserFitnessStatsCreatedDomainEvent.
///
/// WHY it exists:
///   When a user registers their biometrics, we must also calculate their fitness metrics.
///   Instead of doing that inside the same command handler (violating SRP and touching
///   two aggregates), we let the Domain Event drive it reactively.
///
/// HOW it stays atomic:
///   This handler runs INSIDE SaveChangesAsync BEFORE base.SaveChangesAsync().
///   It adds CalculatedMetrics to the SAME DbContext — no separate SaveChanges call.
///   Both UserFitnessStats + CalculatedMetrics commit in ONE atomic transaction.
///
/// If this handler throws → the parent SaveChangesAsync fails → NOTHING is committed.
/// </summary>
public sealed class UserFitnessStatsCreatedDomainEventHandler(
    IAppDbContext context,
    ILogger<UserFitnessStatsCreatedDomainEventHandler> logger
) : INotificationHandler<UserFitnessStatsCreatedDomainEvent>
{
    public async Task Handle(UserFitnessStatsCreatedDomainEvent notification, CancellationToken ct)
    {
        var stats = notification.Stats;

        if (stats is null)
        {
            logger.LogError("UserFitnessStats entity is null in the Domain Event.");
            throw new InvalidOperationException("UserFitnessStats is null in Domain Event.");
        }

        logger.LogInformation("Calculating metrics for UserId: {UserId}", stats.UserId);

        var bmr = FitnessCalculator.CalculateBmr(stats.Weight, stats.Height, stats.Age, stats.Gender);

        var tdeeResult = FitnessCalculator.CalculateTdee(bmr, stats.ActivityLevel);
        if (tdeeResult.IsError)
        {
            throw new InvalidOperationException($"TDEE calculation failed: {tdeeResult.Errors[0].Description}");
        }

        var calorieTargetResult = FitnessCalculator.CalculateCalorieTarget(tdeeResult.Value, stats.Goal);
        if (calorieTargetResult.IsError)
        {
            throw new InvalidOperationException($"CalorieTarget calculation failed: {calorieTargetResult.Errors[0].Description}");
        }

        var status = FitnessCalculator.DetermineStatus(calorieTargetResult.Value);

        var calculationResult = CalculatedMetrics.Create(
            stats.UserId,
            bmr,
            tdeeResult.Value,
            calorieTargetResult.Value,
            status,
            stats.UserId.ToString());

        if (calculationResult.IsError)
        {
            throw new InvalidOperationException($"CalculatedMetrics creation failed: {calculationResult.Errors[0].Description}");
        }

        // Add to the SAME context instance — NO SaveChanges here!
        // The parent SaveChangesAsync will commit UserFitnessStats + CalculatedMetrics together.
        await context.CalculatedMetrics.AddAsync(calculationResult.Value, ct);

        logger.LogInformation("CalculatedMetrics queued for UserId: {UserId}", stats.UserId);
    }
}
