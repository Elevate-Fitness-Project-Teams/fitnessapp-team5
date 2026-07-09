using MediatR;
using Microsoft.AspNetCore.Mvc;
using FCEService.Application.Features.Biometrics.Commands.RegisterUserFitness;
using FCEService.Common;
using FCEService.Presentation.Extensions;

namespace FCEService.Presentation.Endpoints;

public static class BiometricsEndpoints
{
    public static void MapBiometricsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/fitness")
                       .WithTags("Biometrics");

        // POST /api/v1/fitness/weight-goal-activity
        // Stores user biometrics and calculates metrics in a single transaction
        group.MapPost("/{userId:guid}/weight-goal-activity", CreateBiometrics)
        .WithName("CreateBiometrics")
        .WithSummary("Creates a new biometrics.")
        .WithDescription("Adds a new biometrics to the system and calculates metrics.")
        .Produces<ApiResponse<IResult>>(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
        // PUT /api/v1/fitness/stats/{userId}
        group.MapPut("/stats/{userId:guid}", UpdateBiometrics)
        .WithName("UpdateBiometrics")
        .WithSummary("Updates an existing user's biometrics.")
        .WithDescription("Updates biometrics and evaluate plan reassignment.")
        .Produces<ApiResponse<Unit>>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
        // GET /api/v1/fitness/stats/{userId}
        group.MapGet("/stats/{userId:guid}", async (Guid userId, IMediator mediator, CancellationToken ct) =>
        {
            var query = new FCEService.Application.Features.Biometrics.Queries.GetBiometrics.GetBiometricsByIdQuery(userId);
            var result = await mediator.Send(query, ct);
            return result.Match(
                (response) => Results.Ok(response),
                (errors) => errors.ToProblem()
            );
        })
        .WithName("GetBiometrics")
        .WithSummary("Gets an existing user's biometrics.");
    }

    private static async Task<IResult> CreateBiometrics(
        Guid userId,
        [FromBody] RegisterUserFitnessRequest request,
        CancellationToken cancellationToken,
        IMediator mediator)
    {
        var command = new RegisterUserFitnessCommand(
            userId,
            request.Weight,
            request.Height,
            request.BirthDate,
            request.Gender,
            request.Goal,
            request.ActivityLevel
        );

        var result = await mediator.Send(command, cancellationToken);

        return result.Match(
            (response) => Results.Ok(ApiResponse<RegisterUserFitnessResponse>.Success(response, "Biometrics saved.")),
            (errors) => errors.ToProblem()
        );
    }

    private static async Task<IResult> UpdateBiometrics(
        Guid userId,
        [FromBody] UpdateUserFitnessStatsRequest request,
        CancellationToken cancellationToken,
        IMediator mediator)
    {
        var command = new FCEService.Application.Features.Biometrics.Commands.UpdateUserFitnessStats.UpdateUserFitnessStatsCommand(
            userId,
            request.Weight,
            request.Height,
            request.BirthDate,
            request.Gender,
            request.Goal,
            request.ActivityLevel,
            request.RequestNewPlan
        );

        var result = await mediator.Send(command, cancellationToken);

        return result.Match(
            (_) => Results.Ok(ApiResponse<Unit>.Success(Unit.Value, "Biometrics updated successfully.")),
            (errors) => errors.ToProblem()
        );
    }
}

public sealed record RegisterUserFitnessRequest(
    double Weight,
    double Height,
    DateTime BirthDate,
    FCEService.Domain.Enums.Gender Gender,
    FCEService.Domain.Enums.Goal Goal,
    FCEService.Domain.Enums.ActivityLevel ActivityLevel
);

public sealed record UpdateUserFitnessStatsRequest(
    double Weight,
    double Height,
    DateTime BirthDate,
    FCEService.Domain.Enums.Gender Gender,
    FCEService.Domain.Enums.Goal Goal,
    FCEService.Domain.Enums.ActivityLevel ActivityLevel,
    bool RequestNewPlan
);
