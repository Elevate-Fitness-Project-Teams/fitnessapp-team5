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

        // Read from EF Core Change Tracker (in-memory) — no extra DB roundtrips.
        // Both entities were added via AddAsync() inside the Domain Event handlers,
        // so they are already tracked in this DbContext after SaveChanges.
        var metrics = context.CalculatedMetrics
            .Local
            .FirstOrDefault(x => x.UserId == command.UserId);

        if (metrics is null)
        {
            return Error.Unexpected("Metrics.NotFound", "Calculated metrics were not created by the event handler.");
        }

        // Plan is optional — may not exist if no matching plan was seeded
        var assignedPlan = context.UserAssignedPlans
            .Local
            .FirstOrDefault(p => p.UserId == command.UserId && p.IsActive);

        // FitnessPlanConfig was loaded with tracking inside PlanAssignmentService,
        // so it is already in the Change Tracker — no extra DB roundtrip needed.
        var planConfig = context.FitnessPlanConfigs
            .Local
            .FirstOrDefault(c => c.PlanId == assignedPlan!.PlanId);

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
