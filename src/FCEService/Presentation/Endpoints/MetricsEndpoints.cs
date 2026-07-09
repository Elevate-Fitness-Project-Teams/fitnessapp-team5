using FCEService.Application.Features.Biometrics.Queries.GetBiometrics;
using FCEService.Presentation.Extensions;
using MediatR;

namespace FCEService.Presentation.Endpoints;

public static class MetricsEndpoints
{
    public static void MapMetricsEndpoints(this IEndpointRouteBuilder app)
    {
        var metricsGroup = app.MapGroup("/api/v1/fitness")
            .WithTags("Metrics");


        // GET /api/v1/fitness/metrics/{userId}
        metricsGroup.MapGet("/metrics/{userId:guid}", async (Guid userId, IMediator mediator, CancellationToken ct) =>
        {
            var query = new FCEService.Application.Features.Metrics.Queries.GetMetricsByUserId.GetMetricsByUserIdQuery(userId);
            var result = await mediator.Send(query, ct);
            return result.Match(
                (response) => Results.Ok(response),
                (errors) => errors.ToProblem()
            );
        });
    }
}
