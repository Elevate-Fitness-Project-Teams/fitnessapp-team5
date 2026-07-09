using FCEService.Application.Common.Interfaces;
using FCEService.Domain.Entities.CalculatedMetrics;
using FCEService.Domain.Entities.UserAssignedPlan;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FCEService.Application.Features.Biometrics.Events;

/// <summary>
/// Reacts to CalculatedMetricsCreatedDomainEvent.
///
/// Finds the best matching FitnessPlan for the user based on their Status and CalorieTarget,
/// then creates a UserAssignedPlan — all within the same atomic transaction.
///
/// Chain: UserFitnessStats → [Event] → CalculatedMetrics → [Event] → UserAssignedPlan
/// All three commit in ONE SaveChangesAsync call.
/// </summary>
public sealed class CalculatedMetricsCreatedDomainEventHandler(
    IAppDbContext context,
    ILogger<CalculatedMetricsCreatedDomainEventHandler> logger
) : INotificationHandler<CalculatedMetricsCreatedDomainEvent>
{
    public async Task Handle(CalculatedMetricsCreatedDomainEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Assigning fitness plan for UserId: {UserId}, Status: {Status}",
            notification.UserId, notification.Status);

        // Find the plan that matches the user's fitness status and calorie range
        var plan = await context.FitnessPlanConfigs
            .Where(p => p.Status == notification.Status
                     && p.MinCalorie <= notification.CalorieTarget
                     && p.MaxCalorie >= notification.CalorieTarget)
            .FirstOrDefaultAsync(ct);

        // Fallback: match by status only if no calorie-range match
        if (plan is null)
        {
            plan = await context.FitnessPlanConfigs
                .Where(p => p.Status == notification.Status)
                .FirstOrDefaultAsync(ct);
        }

        if (plan is null)
        {
            logger.LogWarning("No FitnessPlan found for Status: {Status}. Plan assignment skipped.", notification.Status);
            return; // Non-critical: user still gets their metrics, plan can be assigned later
        }

        var planResult = UserAssignedPlan.Create(
            notification.UserId,
            plan.PlanId,
            plan.PlanName,
            notification.UserId.ToString());

        if (planResult.IsError)
        {
            logger.LogError("UserAssignedPlan.Create failed for UserId: {UserId}", notification.UserId);
            throw new InvalidOperationException($"Plan assignment failed: {planResult.Errors[0].Description}");
        }

        // Add to SAME context — commits atomically with UserFitnessStats + CalculatedMetrics
        await context.UserAssignedPlans.AddAsync(planResult.Value, ct);

        logger.LogInformation("FitnessPlan {PlanId} queued for UserId: {UserId}", plan.PlanId, notification.UserId);
    }
}
