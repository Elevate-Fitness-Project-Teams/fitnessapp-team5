using FCEService.Application.Common.Interfaces;
using FCEService.Application.Common.Services;
using FCEService.Domain.Entities.CalculatedMetrics;
using FCEService.Domain.Entities.UserFitnessStats;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FCEService.Application.Features.Biometrics.Events;

/// <summary>
/// Reacts to CalculatedMetricsUpdatedDomainEvent.
///
/// Decision Rules (approved):
///   - WeightOnly   → Do NOT reassign plan (progress service weight update)
///   - GoalChanged  → Reassign immediately
///   - UserRequested → Reassign (cooldown already checked in the Command)
///
/// All within the same atomic transaction — NO SaveChanges called here.
/// </summary>
public sealed class CalculatedMetricsUpdatedDomainEventHandler(
    IPlanAssignmentService planAssignmentService,
    ILogger<CalculatedMetricsUpdatedDomainEventHandler> logger
) : INotificationHandler<CalculatedMetricsUpdatedDomainEvent>
{
    public async Task Handle(CalculatedMetricsUpdatedDomainEvent notification, CancellationToken ct)
    {
        logger.LogInformation(
            "CalculatedMetricsUpdated for UserId: {UserId}, Reason: {Reason}",
            notification.UserId, notification.Reason);

        // WeightOnly → metrics updated, plan stays — nothing to do
        if (notification.Reason == FitnessUpdateReason.WeightOnly)
        {
            logger.LogInformation("Reason is WeightOnly — plan reassignment skipped for UserId: {UserId}", notification.UserId);
            return;
        }

        // GoalChanged or UserRequested → Reassign plan
        var result = await planAssignmentService.AssignPlanAsync(
            notification.UserId,
            notification.Goal,
            notification.Status,
            notification.Reason == FitnessUpdateReason.GoalChanged
                ? "Goal changed by user"
                : "User requested plan update",
            ct);

        if (result.IsError)
        {
            logger.LogWarning(
                "Plan reassignment skipped for UserId: {UserId}. Reason: {Error}",
                notification.UserId, result.Errors[0].Description);
            // Non-critical — user still has their updated metrics
            return;
        }

        logger.LogInformation("Plan reassigned for UserId: {UserId}", notification.UserId);
    }
}
