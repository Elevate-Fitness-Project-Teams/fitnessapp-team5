using FCEService.Presentation.Extensions;
using MediatR;
using FCEService.Application.Common.Interfaces;

namespace FCEService.Presentation.Endpoints;

public static class PlanEndpoints
{
    public static void MapPlanEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/fitness")
                       .WithTags("Plans")
                       .RequireAuthorization() // Fix #10: BOLA prevention
                       .RequireRateLimiting("fce_api");

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

        // GET /api/v1/fitness/assigned-plan
        group.MapGet("/assigned-plan", async (ICurrentUser currentUser, IMediator mediator, CancellationToken ct) =>
        {
            var query = new FCEService.Application.Features.Plans.Queries.GetUserAssignedPlan.GetUserAssignedPlanQuery(currentUser.Id);
            var result = await mediator.Send(query, ct);
            return result.Match(
                (response) => Results.Ok(response),
                (errors) => errors.ToProblem()
            );
        });

        // POST /api/v1/fitness/assign-plan
        group.MapPost("/assign-plan", async (ICurrentUser currentUser, IMediator mediator, CancellationToken ct) =>
        {
            var command = new FCEService.Application.Features.Plans.Commands.AssignPlan.AssignPlanCommand(currentUser.Id);
            var result = await mediator.Send(command, ct);
            return result.Match(
                (_) => Results.Created("/api/v1/fitness/assigned-plan",
                    FCEService.Common.ApiResponse<Unit>.Success(Unit.Value, "Plan assigned successfully.")),
                (errors) => errors.ToProblem()
            );
        })
        .WithName("AssignPlan")
        .WithSummary("Assigns a fitness plan to the current user.")
        .Produces<FCEService.Common.ApiResponse<Unit>>(StatusCodes.Status201Created)
        .Produces<Microsoft.AspNetCore.Mvc.ProblemDetails>(StatusCodes.Status400BadRequest);
    }
}
