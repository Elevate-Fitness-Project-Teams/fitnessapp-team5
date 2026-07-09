using FCEService.Domain.Common;
using FCEService.Domain.Enums;
using FCEService.Domain.Entities.UserFitnessStats;
using System;

namespace FCEService.Domain.Entities.CalculatedMetrics;

/// <summary>
/// Raised when CalculatedMetrics are created for a user.
/// The PlanAssignmentHandler listens to this event and assigns the appropriate FitnessPlan.
/// </summary>
public sealed class CalculatedMetricsCreatedDomainEvent : DomainEvent
{
    public Guid UserId { get; }
    public FitnessStatus Status { get; }
    public double CalorieTarget { get; }

    public CalculatedMetricsCreatedDomainEvent(Guid userId, FitnessStatus status, double calorieTarget)
    {
        UserId = userId;
        Status = status;
        CalorieTarget = calorieTarget;
    }
}

/// <summary>
/// Raised when CalculatedMetrics are updated (triggered by weight or stats update).
/// PlanReassignmentHandler uses Reason to decide whether to assign a new plan.
/// </summary>
public sealed class CalculatedMetricsUpdatedDomainEvent : DomainEvent
{
    public Guid UserId { get; }
    public FitnessStatus Status { get; }
    public double CalorieTarget { get; }
    public FitnessUpdateReason Reason { get; }
    public Goal Goal { get; }

    public CalculatedMetricsUpdatedDomainEvent(
        Guid userId, 
        FitnessStatus status, 
        double calorieTarget,
        FitnessUpdateReason reason,
        Goal goal)
    {
        UserId = userId;
        Status = status;
        CalorieTarget = calorieTarget;
        Reason = reason;
        Goal = goal;
    }
}
