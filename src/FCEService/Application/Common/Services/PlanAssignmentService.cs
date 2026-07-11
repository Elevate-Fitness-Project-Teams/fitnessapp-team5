using FCEService.Application.Common.Interfaces;
using FCEService.Domain.Common.Results;
using FCEService.Domain.Entities.FitnessPlanConfig;
using FCEService.Domain.Entities.UserAssignedPlan;
using FCEService.Domain.Entities.UserPlanHistory;
using FCEService.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FCEService.Application.Common.Services;

public sealed class PlanAssignmentService(IAppDbContext db) : IPlanAssignmentService
{
    public async Task<Result<UserAssignedPlan>> AssignPlanAsync(
        Guid userId,
        Goal goal,
        FitnessStatus status,
        string reasonForChange,
        CancellationToken ct,
        string modifiedBy = "system")   // explicit actor for audit — "system" for background jobs, userId for user actions
    {
        var config = await db.FitnessPlanConfigs
            .FirstOrDefaultAsync(x => x.Goal == goal && x.Status == status, ct);

        if (config is null)
            return FitnessPlanConfigErrors.NotFound;

        var oldPlan = await db.UserAssignedPlans
            .FirstOrDefaultAsync(x => x.UserId == userId && x.IsActive, ct);

        // Same plan already active — nothing to change
        if (oldPlan is not null && oldPlan.PlanId == config.PlanId)
            return oldPlan;

        // Deactivate old plan and write history
        if (oldPlan is not null)
        {
            oldPlan.Deactivate(modifiedBy);

            var historyResult = UserPlanHistory.Create(userId, oldPlan.PlanId, reasonForChange, modifiedBy);
            if (historyResult.IsError)
                return historyResult.Errors;

            historyResult.Value.End(DateTime.UtcNow, modifiedBy);
            await db.UserPlanHistories.AddAsync(historyResult.Value, ct);
        }

        // Assign new plan
        var newPlanResult = UserAssignedPlan.Create(userId, config.PlanId, config.PlanName, modifiedBy);
        if (newPlanResult.IsError)
            return newPlanResult.Errors;

        await db.UserAssignedPlans.AddAsync(newPlanResult.Value, ct);

        // CONTRACT: SaveChangesAsync is intentionally NOT called here.
        // This service only stages entities in the EF Core Change Tracker.
        // The CALLER (handler or background job) is responsible for committing,
        // allowing it to batch multiple plan assignments in a single transaction.
        return newPlanResult.Value;
    }
}
