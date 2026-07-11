using FCEService.Application.Features.Metrics.Queries.GetMetricsByUserId;
using FCEService.Application.Common.Interfaces;
using FCEService.Presentation.Extensions;
using MediatR;

namespace FCEService.Presentation.Endpoints;

public static class MetricsEndpoints
{
    public static void MapMetricsEndpoints(this IEndpointRouteBuilder app)
    {
        var metricsGroup = app.MapGroup("/api/v1/fitness")
            .WithTags("Metrics")
            .RequireAuthorization()
            .RequireRateLimiting("fce_api");

        // GET /api/v1/fitness/metrics
        metricsGroup.MapGet("/metrics", async (ICurrentUser currentUser, IMediator mediator, CancellationToken ct) =>
        {
            var query = new GetMetricsByUserIdQuery(currentUser.Id);
            var result = await mediator.Send(query, ct);
            return result.Match(
                (response) => Results.Ok(response),
                (errors) => errors.ToProblem()
            );
        })
        .WithName("GetMetrics")
        .WithSummary("Gets calculated fitness metrics for the current user.")
        .WithDescription("Returns BMR, TDEE, calorie target and fitness status for the authenticated user.")
        .Produces<GetMetricsByUserIdResponse>(StatusCodes.Status200OK)
        .Produces<Microsoft.AspNetCore.Mvc.ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<Microsoft.AspNetCore.Mvc.ProblemDetails>(StatusCodes.Status404NotFound);
    }
}
