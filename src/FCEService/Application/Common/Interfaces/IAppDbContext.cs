namespace FCEService.Application.Common.Interfaces;

using FCEService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public interface IAppDbContext
{
    DbSet<FCEService.Domain.Entities.UserFitnessStats.UserFitnessStats> UserFitnessStats { get; }  
    DbSet<FCEService.Domain.Entities.CalculatedMetrics.CalculatedMetrics> CalculatedMetrics { get; }  
    DbSet<FCEService.Domain.Entities.FitnessPlanConfig.FitnessPlanConfig> FitnessPlanConfigs { get; }  
    DbSet<FCEService.Domain.Entities.UserAssignedPlan.UserAssignedPlan> UserAssignedPlans { get; }  
    DbSet<FCEService.Domain.Entities.UserPlanHistory.UserPlanHistory> UserPlanHistories { get; }  

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
