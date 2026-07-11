using FCEService.Domain.Entities.UserAssignedPlan;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FCEService.Application.Features.Biometrics.Events;

/// <summary>
/// Reacts to UserAssignedPlanCreatedDomainEvent.
///
/// FCE RULE: Consumer Only — no publishing to RabbitMQ.
/// This handler records the plan assignment in-process.
/// If downstream services need to know about plan assignments,
/// they should query the FCE HTTP endpoint (sync) or
/// a separate Publishing Service should handle that concern.
/// </summary>
public sealed class UserAssignedPlanCreatedDomainEventHandler(
    ILogger<UserAssignedPlanCreatedDomainEventHandler> logger
) : INotificationHandler<UserAssignedPlanCreatedDomainEvent>
{
    public Task Handle(UserAssignedPlanCreatedDomainEvent notification, CancellationToken ct)
    {
        logger.LogInformation(
            "UserAssignedPlan created in-process for UserId: {UserId}, PlanId: {PlanId}",
            notification.UserId, notification.PlanId);

      
        return Task.CompletedTask;
    }
}
