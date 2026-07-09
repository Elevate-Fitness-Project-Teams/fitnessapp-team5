using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FCEService.Application.Common.Interfaces;
using FCEService.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using FCEService.Application.Common.Mappings;

namespace FCEService.Application.Features.Plans.Queries.GetPlanConfigs;

public sealed class GetPlanConfigsQueryHandler(
    IAppDbContext context,
    ILogger<GetPlanConfigsQueryHandler> logger
) : IRequestHandler<GetPlanConfigsQuery, Result<GetPlanConfigsResponse>>
{
    public async Task<Result<GetPlanConfigsResponse>> Handle(GetPlanConfigsQuery request, CancellationToken ct)
    {
        logger.LogInformation("Retrieving all fitness plan configurations.");

        var totalCount = await context.FitnessPlanConfigs.CountAsync(ct);

        var plans = await context.FitnessPlanConfigs
            .AsNoTracking()
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        var dtos = plans.Select(p => p.ToDto()).ToList();

        return new GetPlanConfigsResponse(dtos, totalCount, request.PageNumber, request.PageSize);
    }
}
