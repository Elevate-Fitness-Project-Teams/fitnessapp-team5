using System;
using FCEService.Domain.Common;

namespace FCEService.Domain.Entities.UserFitnessStats
{
    public sealed class UserFitnessStatsCreatedDomainEvent : DomainEvent
    {
        public UserFitnessStats UserFitnessStats { get; }

        public UserFitnessStatsCreatedDomainEvent(UserFitnessStats userFitnessStats)
        {
            UserFitnessStats = userFitnessStats;
        }
    }

    public sealed class UserFitnessStatsUpdatedDomainEvent : DomainEvent
    {
        public UserFitnessStats UserFitnessStats { get; }

        public UserFitnessStatsUpdatedDomainEvent(UserFitnessStats userFitnessStats)
        {
            UserFitnessStats = userFitnessStats;
        }
    }
}
