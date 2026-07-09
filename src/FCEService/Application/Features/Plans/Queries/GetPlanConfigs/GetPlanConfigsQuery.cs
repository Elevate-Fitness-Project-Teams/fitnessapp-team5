using FCEService.Domain.Common.Results;
using MediatR;

namespace FCEService.Application.Features.Plans.Queries.GetPlanConfigs;

using FCEService.Application.Common.Caching;

public sealed record GetPlanConfigsQuery(int PageNumber = 1, int PageSize = 10) : ICacheableQuery<Result<GetPlanConfigsResponse>>
{
    public string CacheKey => $"PlanConfigs_{PageNumber}_{PageSize}";
    public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
}
