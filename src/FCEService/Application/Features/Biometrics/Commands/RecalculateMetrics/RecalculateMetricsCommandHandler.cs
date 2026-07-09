using FCEService.Application.Common.Interfaces;
using FCEService.Domain.Common.Results;
using FCEService.Domain.Entities.UserFitnessStats;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace FCEService.Application.Features.Biometrics.Commands.RecalculateMetrics;

/// <summary>
/// Handles weight updates from Progress Service.
///
/// Touches ONE entity: UserFitnessStats.
/// The UserFitnessStatsUpdatedDomainEvent (WeightOnly reason) fires via SaveChangesAsync,
/// which triggers recalculation of CalculatedMetrics — WITHOUT touching the plan.
/// </summary>
internal sealed class RecalculateMetricsCommandHandler(IAppDbContext db)
    : IRequestHandler<RecalculateMetricsCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(RecalculateMetricsCommand request, CancellationToken cancellationToken)
    {
        // Load both entities in parallel — one round trip to DB instead of two.
        // CalculatedMetrics is loaded into EF Core's Identity Map here so the
        // UserFitnessStatsUpdatedDomainEventHandler gets it from cache (no extra DB query).
        var statsTask = db.UserFitnessStats
            .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

        var metricsTask = db.CalculatedMetrics
            .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

        await Task.WhenAll(statsTask, metricsTask);

        if (statsTask.Result is null)
            return UserFitnessStatsErrors.UserNotFound;

        if (metricsTask.Result is null)
            return Domain.Entities.CalculatedMetrics.CalculatedMetricsErrors.NotFound;

        // Update Weight — raises UserFitnessStatsUpdatedDomainEvent(WeightOnly)
        var updateResult = statsTask.Result.UpdateWeight(request.NewWeight);
        if (updateResult.IsError)
            return updateResult.Errors;

        // Save — Domain Events fire automatically:
        //   UserFitnessStatsUpdatedDomainEvent → event handler finds CalculatedMetrics
        //   from EF Core Identity Map (no DB query) → updates metrics
        //   CalculatedMetricsUpdatedDomainEvent(WeightOnly) → skips plan assignment
        await db.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
