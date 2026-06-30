using MediatR;

namespace FCEService.Presentation.Endpoints;

public static class PlanEndpoints
{
    public static void MapPlanEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/fitness")
                       .WithTags("Plans");
                       //.RequireAuthorization();

        // POST /api/v1/fitness/assign-plan
        group.MapPost("/assign-plan", async (IMediator mediator) =>
        {
            throw new NotImplementedException();
        });

        // PUT /api/v1/fitness/recalculate/{userId}
        group.MapPut("/recalculate/{userId:int}", async (int userId, IMediator mediator) =>
        {
            throw new NotImplementedException();
        });

        // GET /api/v1/fitness/plan-configs
        group.MapGet("/plan-configs", async (IMediator mediator) =>
        {
            throw new NotImplementedException();
        });

        // GET /api/v1/fitness/plans/{planId}
        group.MapGet("/plans/{planId}", async (string planId, IMediator mediator) =>
        {
            throw new NotImplementedException();
        });
    }
}
