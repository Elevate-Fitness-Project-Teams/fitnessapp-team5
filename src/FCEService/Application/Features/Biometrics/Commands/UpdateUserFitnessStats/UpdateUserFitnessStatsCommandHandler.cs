using FCEService.Application.Common.Interfaces;
using FCEService.Domain.Common.Results;
using FCEService.Domain.Entities.CalculatedMetrics;
using FCEService.Domain.Entities.UserFitnessStats;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FCEService.Application.Features.Biometrics.Commands.UpdateUserFitnessStats;

/// <summary>
/// Handles manual stats updates by the user.
/// Touches ONE entity: UserFitnessStats.
/// Domain Events handle the rest:
///   UserFitnessStatsUpdatedDomainEvent → recalculate CalculatedMetrics
///   CalculatedMetricsUpdatedDomainEvent(reason) → conditionally reassign plan
/// </summary>
internal sealed class UpdateUserFitnessStatsCommandHandler(IAppDbContext db)
    : IRequestHandler<UpdateUserFitnessStatsCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(UpdateUserFitnessStatsCommand request, CancellationToken cancellationToken)
    {
        // Preload all needed entities in parallel — one connection round trip.
        // CalculatedMetrics + UserAssignedPlan are cached in EF Core's Identity Map
        // so downstream event handlers don't need extra DB queries.
        var statsTask = db.UserFitnessStats
            .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

        var metricsTask = db.CalculatedMetrics
            .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

        var activePlanTask = db.UserAssignedPlans
            .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.IsActive, cancellationToken);

        await Task.WhenAll(statsTask, metricsTask, activePlanTask);

        if (statsTask.Result is null)
            return UserFitnessStatsErrors.UserNotFound;

        if (metricsTask.Result is null)
            return CalculatedMetricsErrors.NotFound;

        var stats = statsTask.Result;
        var activePlan = activePlanTask.Result;

        // Determine reason BEFORE updating
        FitnessUpdateReason reason;

        if (stats.Goal != request.Goal)
        {
            reason = FitnessUpdateReason.GoalChanged;
        }
        else if (request.RequestNewPlan)
        {
            bool cooldownPassed = activePlan is null ||
                                  (DateTimeOffset.UtcNow - activePlan.CreatedAtUtc).TotalDays >= 14;

            reason = cooldownPassed ? FitnessUpdateReason.UserRequested : FitnessUpdateReason.WeightOnly;
        }
        else
        {
            reason = FitnessUpdateReason.WeightOnly;
        }

        // Update stats — raises UserFitnessStatsUpdatedDomainEvent(reason)
        var updateResult = stats.Update(
            request.Weight,
            request.Height,
            request.BirthDate,
            request.Gender,
            request.Goal,
            request.ActivityLevel,
            "user",
            reason);

        if (updateResult.IsError)
            return updateResult.Errors;

        // Save — Domain Events fire automatically via interceptor.
        // Event handlers resolve CalculatedMetrics + UserAssignedPlan from Identity Map — no extra DB queries.
        await db.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
