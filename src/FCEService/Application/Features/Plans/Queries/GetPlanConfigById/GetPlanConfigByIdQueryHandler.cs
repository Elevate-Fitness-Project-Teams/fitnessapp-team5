using System.Threading;
using System.Threading.Tasks;
using FCEService.Application.Common.Interfaces;
using FCEService.Domain.Common.Results;
using FCEService.Domain.Entities.FitnessPlanConfig;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FCEService.Application.Common.Mappings;

namespace FCEService.Application.Features.Plans.Queries.GetPlanConfigById;

public sealed class GetPlanConfigByIdQueryHandler(
    IAppDbContext context,
    ILogger<GetPlanConfigByIdQueryHandler> logger
) : IRequestHandler<GetPlanConfigByIdQuery, Result<GetPlanConfigByIdResponse>>
{
    public async Task<Result<GetPlanConfigByIdResponse>> Handle(GetPlanConfigByIdQuery request, CancellationToken ct)
    {
        logger.LogInformation("Retrieving fitness plan configuration for Id: {PlanId}", request.PlanId);

        var plan = await context.FitnessPlanConfigs
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PlanId == request.PlanId, ct);

        if (plan is null)
        {
            logger.LogWarning("Fitness plan configuration with Id: {PlanId} not found.", request.PlanId);
            return FitnessPlanConfigErrors.NotFound;
        }

        return plan.ToResponse();
    }
}
