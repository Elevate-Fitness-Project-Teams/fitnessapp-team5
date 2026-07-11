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
internal sealed class UpdateUserFitnessStatsCommandHandler(IAppDbContext db, ICurrentUser currentUser)
    : IRequestHandler<UpdateUserFitnessStatsCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(UpdateUserFitnessStatsCommand request, CancellationToken cancellationToken)
    {
        // Sequential awaits — DbContext is NOT thread-safe.
        // Task.WhenAll on the same DbContext crashes under concurrent load
        // with: "A second operation was started on this context instance"
        var stats = await db.UserFitnessStats
            .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

        var metrics = await db.CalculatedMetrics
            .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

        var activePlan = await db.UserAssignedPlans
            .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.IsActive, cancellationToken);


        if (stats is null)
            return UserFitnessStatsErrors.UserNotFound;

        if (metrics is null)
            return CalculatedMetricsErrors.NotFound;

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
            currentUser.Id.ToString(),
            reason);

        if (updateResult.IsError)
            return updateResult.Errors;

        // Save — Domain Events fire automatically via interceptor.
        // Event handlers resolve CalculatedMetrics + UserAssignedPlan from Identity Map — no extra DB queries.
        await db.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
