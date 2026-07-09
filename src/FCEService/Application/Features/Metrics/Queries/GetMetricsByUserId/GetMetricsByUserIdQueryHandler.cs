using FCEService.Application.Common.Interfaces;
using FCEService.Domain.Common.Results;
using FCEService.Domain.Entities.CalculatedMetrics;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using FCEService.Application.Common.Mappings;

namespace FCEService.Application.Features.Metrics.Queries.GetMetricsByUserId;

public sealed class GetMetricsByUserIdQueryHandler(
    IAppDbContext context,
    ILogger<GetMetricsByUserIdQueryHandler> logger
) : IRequestHandler<GetMetricsByUserIdQuery, Result<GetMetricsByUserIdResponse>>
{
    public async Task<Result<GetMetricsByUserIdResponse>> Handle(GetMetricsByUserIdQuery request, CancellationToken ct)
    {
        logger.LogInformation("Getting latest CalculatedMetrics for UserId: {UserId}", request.UserId);

        // Fix #3: AsNoTracking — read-only query, no mutation needed
        var metrics = await context.CalculatedMetrics
            .AsNoTracking()
            .Where(m => m.UserId == request.UserId)
            .OrderByDescending(m => m.CreatedAtUtc)
            .FirstOrDefaultAsync(ct);

        if (metrics is null)
        {
            logger.LogWarning("No CalculatedMetrics found for UserId: {UserId}", request.UserId);
            return CalculatedMetricsErrors.NotFound; // Re-use NotFound error
        }

        return metrics.ToResponse();
    }
}
