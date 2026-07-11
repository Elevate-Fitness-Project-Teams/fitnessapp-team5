using FCEService.Application.Common.Interfaces;
using FCEService.Domain.Entities.CalculatedMetrics;
using FCEService.Domain.Entities.UserFitnessStats;
using FCEService.Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FCEService.Application.Features.Biometrics.Events;

/// <summary>
/// Reacts to UserFitnessStatsUpdatedDomainEvent.
/// Recalculates BMR, TDEE, CalorieTarget and updates CalculatedMetrics.
/// Fires CalculatedMetricsUpdatedDomainEvent carrying the reason for the next handler.
/// Runs within the same atomic transaction — no SaveChanges here.
/// </summary>
public sealed class UserFitnessStatsUpdatedDomainEventHandler(
    IAppDbContext context,
    ILogger<UserFitnessStatsUpdatedDomainEventHandler> logger
) : INotificationHandler<UserFitnessStatsUpdatedDomainEvent>
{
    public async Task Handle(UserFitnessStatsUpdatedDomainEvent notification, CancellationToken ct)
    {
        var stats = notification.Stats;

        logger.LogInformation("Recalculating metrics for UserId: {UserId}, Reason: {Reason}",
            stats.UserId, notification.Reason);

        var metrics = await context.CalculatedMetrics
            .FirstOrDefaultAsync(x => x.UserId == stats.UserId, ct);

        if (metrics is null)
            throw new InvalidOperationException($"CalculatedMetrics not found for UserId {stats.UserId}.");

        var bmr = FitnessCalculator.CalculateBmr(stats.Weight, stats.Height, stats.Age, stats.Gender);

        var tdeeResult = FitnessCalculator.CalculateTdee(bmr, stats.ActivityLevel);
        if (tdeeResult.IsError)
            throw new InvalidOperationException($"TDEE calculation failed: {tdeeResult.Errors[0].Description}");

        var calorieTargetResult = FitnessCalculator.CalculateCalorieTarget(tdeeResult.Value, stats.Goal);
        if (calorieTargetResult.IsError)
            throw new InvalidOperationException($"CalorieTarget calculation failed: {calorieTargetResult.Errors[0].Description}");

        var status = FitnessCalculator.DetermineStatus(calorieTargetResult.Value);

        var updateResult = metrics.Update(
            bmr,
            tdeeResult.Value,
            calorieTargetResult.Value,
            status,
            stats.UserId.ToString(),
            notification.Reason,
            stats.Goal);

        if (updateResult.IsError)
            throw new InvalidOperationException($"CalculatedMetrics update failed: {updateResult.Errors[0].Description}");

        logger.LogInformation("CalculatedMetrics updated for UserId: {UserId}", stats.UserId);
    }
}
