using FCEService.Application.Common.Interfaces;
using FCEService.Application.Common.Services;
using FCEService.Domain.Common.Results;
using FCEService.Domain.Entities.CalculatedMetrics;
using FCEService.Domain.Entities.UserFitnessStats;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace FCEService.Application.Features.Plans.Commands.AssignPlan;

internal sealed class AssignPlanCommandHandler(
    IAppDbContext db,
    IPlanAssignmentService planAssignmentService
) : IRequestHandler<AssignPlanCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(AssignPlanCommand request, CancellationToken cancellationToken)
    {
        // 1. Fetch current stats and metrics to know what plan to assign
        var statsTask = db.UserFitnessStats
            .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

        var metricsTask = db.CalculatedMetrics
            .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

        await Task.WhenAll(statsTask, metricsTask);

        if (statsTask.Result is null)
            return UserFitnessStatsErrors.UserNotFound;

        if (metricsTask.Result is null)
            return CalculatedMetricsErrors.NotFound;

        var stats = statsTask.Result;
        var metrics = metricsTask.Result;

        // 2. Assign the plan directly (bypassing the 14-day cooldown for manual assignment)
        var assignResult = await planAssignmentService.AssignPlanAsync(
            request.UserId,
            stats.Goal,
            metrics.Status,
            "Direct manual plan assignment",
            cancellationToken);

        if (assignResult.IsError)
            return assignResult.Errors;

        // 3. Save changes
        await db.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
