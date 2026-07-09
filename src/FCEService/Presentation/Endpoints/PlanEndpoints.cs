using FCEService.Presentation.Extensions;
using MediatR;

namespace FCEService.Presentation.Endpoints;

public static class PlanEndpoints
{
    public static void MapPlanEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/fitness")
                       .WithTags("Plans");

        // GET /api/v1/fitness/plan-configs
        group.MapGet("/plan-configs", async (int? pageNumber, int? pageSize, IMediator mediator, CancellationToken ct) =>
        {
            var query = new FCEService.Application.Features.Plans.Queries.GetPlanConfigs.GetPlanConfigsQuery(pageNumber ?? 1, pageSize ?? 10);
            var result = await mediator.Send(query, ct);
            return result.Match(
                (response) => Results.Ok(response),
                (errors) => errors.ToProblem()
            );
        });

        // GET /api/v1/fitness/plans/{planId}
        group.MapGet("/plans/{planId:guid}", async (Guid planId, IMediator mediator, CancellationToken ct) =>
        {
            var query = new FCEService.Application.Features.Plans.Queries.GetPlanConfigById.GetPlanConfigByIdQuery(planId);
            var result = await mediator.Send(query, ct);
            return result.Match(
                (response) => Results.Ok(response),
                (errors) => errors.ToProblem()
            );
        });

        // GET /api/v1/fitness/{userId}/assigned-plan
        group.MapGet("/{userId:guid}/assigned-plan", async (Guid userId, IMediator mediator, CancellationToken ct) =>
        {
            var query = new FCEService.Application.Features.Plans.Queries.GetUserAssignedPlan.GetUserAssignedPlanQuery(userId);
            var result = await mediator.Send(query, ct);
            return result.Match(
                (response) => Results.Ok(response),
                (errors) => errors.ToProblem()
            );
        });

        // POST /api/v1/fitness/assign-plan
        group.MapPost("/assign-plan", async (Microsoft.AspNetCore.Http.HttpContext httpContext, IMediator mediator, CancellationToken ct) =>
        {
            var userIdString = httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
             if (!Guid.TryParse(userIdString, out var userId))
                return Results.Unauthorized();

            var command = new FCEService.Application.Features.Plans.Commands.AssignPlan.AssignPlanCommand(userId);
            var result = await mediator.Send(command, ct);
            return result.Match(
                (_) => Results.Ok(FCEService.Common.ApiResponse<Unit>.Success(Unit.Value, "Plan assigned successfully.")),
                (errors) => errors.ToProblem()
            );
        });
    }
}

public sealed record AssignPlanRequest(Guid UserId);
