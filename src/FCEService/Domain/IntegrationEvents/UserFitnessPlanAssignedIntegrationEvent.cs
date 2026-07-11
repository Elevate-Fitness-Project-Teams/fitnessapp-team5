namespace FCEService.Domain.IntegrationEvents;

/// <summary>
/// Integration Event — published to the Message Broker (via MassTransit Outbox).
/// 
/// This is the PUBLIC CONTRACT that other Microservices subscribe to:
///   - Notification Service  → sends welcome email/push notification
///   - Workout Service       → generates the workout plan for this user
///   - Nutrition Service     → generates the meal plan for this user
///
/// IMPORTANT: This record is SERIALIZED and sent over the wire.
/// Never put Domain concepts (Enums, Value Objects) here — use primitives only.
/// </summary>
public sealed record UserFitnessPlanAssignedIntegrationEvent(
    Guid UserId,
    Guid PlanId,
    string PlanName,
    DateTimeOffset AssignedAt);
