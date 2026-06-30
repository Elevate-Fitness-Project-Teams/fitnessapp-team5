using FCEService.Application.Features.Biometrics.Queries.GetBiometrics;
using FCEService.Presentation.Extensions;
using MediatR;

namespace FCEService.Presentation.Endpoints;

public static class MetricsEndpoints
{
    public static void MapMetricsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/fitness")
                       .WithTags("Metrics");
                       //.RequireAuthorization();

        // POST /api/v1/fitness/calculate
        group.MapPost("/calculate", async (IMediator mediator) =>
        {
            throw new NotImplementedException();
        });

        // GET /api/v1/fitness/metrics/{userId}
        group.MapGet("/metrics/{userId:int}", async (int userId, IMediator mediator) =>
        {
            throw new NotImplementedException();
        });

        // GET /api/v1/fitness/stats/{id}
        group.MapGet("/stats/{id:guid}", async (Guid id, IMediator mediator, CancellationToken ct) =>
        {
            var query = new GetBiometricsByIdQuery(id);
            var result = await mediator.Send(query, ct);
            return result.Match(
                (response) => Results.Ok(response),
                (errors) => errors.ToProblem()
            );
        });
    }
}
