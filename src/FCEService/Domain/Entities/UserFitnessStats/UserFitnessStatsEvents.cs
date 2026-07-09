using System;
using FCEService.Domain.Common;

namespace FCEService.Domain.Entities.UserFitnessStats
{
    public enum FitnessUpdateReason
    {
        WeightOnly,       // From Progress Service — do NOT touch plan
        GoalChanged,      // User changed Goal — reassign plan immediately
        UserRequested     // User manually updated + clicked "Request New Plan" after cooldown
    }

    public sealed class UserFitnessStatsCreatedDomainEvent : DomainEvent
    {
        public UserFitnessStats Stats { get; }

        public UserFitnessStatsCreatedDomainEvent(UserFitnessStats stats)
        {
            Stats = stats;
        }
    }

    public sealed class UserFitnessStatsUpdatedDomainEvent : DomainEvent
    {
        public UserFitnessStats Stats { get; }
        public FitnessUpdateReason Reason { get; }

        public UserFitnessStatsUpdatedDomainEvent(UserFitnessStats stats, FitnessUpdateReason reason)
        {
            Stats = stats;
            Reason = reason;
        }
    }
}
