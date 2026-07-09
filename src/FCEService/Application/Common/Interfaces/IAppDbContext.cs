using FCEService.Domain.Entities.CalculatedMetrics;
using FCEService.Domain.Entities.FitnessPlanConfig;
using FCEService.Domain.Entities.UserAssignedPlan;
using FCEService.Domain.Entities.UserFitnessStats;
using FCEService.Domain.Entities.UserPlanHistory;
using Microsoft.EntityFrameworkCore;

namespace FCEService.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<UserFitnessStats> UserFitnessStats { get; }
    DbSet<CalculatedMetrics> CalculatedMetrics { get; }
    DbSet<FitnessPlanConfig> FitnessPlanConfigs { get; }
    DbSet<UserAssignedPlan> UserAssignedPlans { get; }
    DbSet<UserPlanHistory> UserPlanHistories { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
