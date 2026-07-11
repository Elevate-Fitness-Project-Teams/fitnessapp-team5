using FCEService.Application.Common.Interfaces;
using FCEService.Domain.Common.Results;
using FCEService.Domain.Entities.UserFitnessStats;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FCEService.Application.Features.Biometrics.Commands.RegisterUserFitness;

/// <summary>
/// APPROACH 2 — Domain Events (see approach-1-orchestrator tag for the MediatR chaining version)
///
/// This handler only touches ONE aggregate: UserFitnessStats.
/// The UserFitnessStatsCreatedDomainEvent fires automatically inside Create().
/// The EventHandler reacts to it and creates CalculatedMetrics in the same transaction.
/// </summary>
public sealed class RegisterUserFitnessCommandHandler(
    IAppDbContext context
) : IRequestHandler<RegisterUserFitnessCommand, Result<RegisterUserFitnessResponse>>
{
    public async Task<Result<RegisterUserFitnessResponse>> Handle(RegisterUserFitnessCommand command, CancellationToken ct)
    {
        // Guard: one user → one fitness record
        var exists = await context.UserFitnessStats
            .AnyAsync(x => x.UserId == command.UserId, ct);

        if (exists)
        {
            return UserFitnessStatsErrors.UserAlreadyExists;
        }

        // Create the Aggregate Root — raises UserFitnessStatsCreatedDomainEvent internally
        var entityResult = UserFitnessStats.Create(
            command.UserId,
            command.Weight,
            command.Height,
            command.BirthDate,
            command.Gender,
            command.Goal,
            command.ActivityLevel);

        if (entityResult.IsError)
        {
            return entityResult.Errors;
        }

        // Persist:
        //   1. context.ExecuteAsync opens a DB transaction
        //   2. AddAsync adds UserFitnessStats to the EF context (memory only)
        //   3. ExecuteAsync calls SaveChangesAsync on success
        //   4. SaveChangesAsync dispatches the Domain Event FIRST
        //   5. EventHandler adds CalculatedMetrics to the SAME context
        //   6. base.SaveChangesAsync commits BOTH entities atomically
        var transactionResult = await context.ExecuteAsync(async () =>
        {
            await context.UserFitnessStats.AddAsync(entityResult.Value, ct);
            return Result.Success;
        });

        if (transactionResult.IsError)
        {
            return transactionResult.Errors;
        }

        // Query DB directly — more reliable than reading from .Local Change Tracker.
        // The event handlers have already committed both UserFitnessStats + CalculatedMetrics.
        var metrics = await context.CalculatedMetrics
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == command.UserId, ct);

        if (metrics is null)
        {
            return Error.Unexpected("Metrics.NotFound", "Calculated metrics were not created by the event handler.");
        }

        // Plan is optional — may not exist if no matching plan config was found
        var assignedPlan = await context.UserAssignedPlans
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == command.UserId && p.IsActive, ct);

        // Only load plan config if a plan was actually assigned
        // Bug fix: was using assignedPlan!.PlanId which throws NullReferenceException when assignedPlan is null
        var planConfig = assignedPlan is not null
            ? await context.FitnessPlanConfigs
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.PlanId == assignedPlan.PlanId, ct)
            : null;

        return new RegisterUserFitnessResponse(
            metrics.UserId,
            metrics.Bmr,
            metrics.Tdee,
            metrics.CalorieTarget,
            metrics.Status,
            assignedPlan?.PlanId,
            planConfig?.PlanName);
    }
}
