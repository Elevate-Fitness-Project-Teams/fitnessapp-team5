using FCEService.Domain.Common;

namespace FCEService.Domain.Entities.UserAssignedPlan;

/// <summary>
/// Domain Event — raised internally when a plan is assigned to a user.
/// LAYER: Domain Layer — In-process only, never serialized.
/// 
/// This is the BRIDGE between the internal Domain chain and the external world.
/// The handler publishes a MassTransit Integration Event to the Outbox.
/// </summary>
public sealed class UserAssignedPlanCreatedDomainEvent : DomainEvent
{
    public Guid UserId { get; }
    public Guid PlanId { get; }
    public string PlanName { get; }

    public UserAssignedPlanCreatedDomainEvent(Guid userId, Guid planId, string planName)
    {
        UserId = userId;
        PlanId = planId;
        PlanName = planName;
    }
}
