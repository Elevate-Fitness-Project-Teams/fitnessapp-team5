using MediatR;
using Microsoft.AspNetCore.Mvc;
using FCEService.Application.Features.Biometrics.Commands.IngestBiometrics;
using FCEService.Common;
using FCEService.Presentation.Extensions;

namespace FCEService.Presentation.Endpoints;

public static class BiometricsEndpoints
{
    public static void MapBiometricsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/fitness")
                       .WithTags("Biometrics");
                       //.RequireAuthorization();

        // POST /api/v1/fitness/weight-goal-activity
        // Stores user biometrics: weight, height, age, gender, goal, activity level
        group.MapPost("/weight-goal-activity", CreateBiometrics)
        .WithName("CreateBiometrics")
        .WithSummary("Creates a new biometrics.")
        .WithDescription("Adds a new biometrics to the system.")
        .Produces<ApiResponse<IResult>>(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);



    }

    private static async Task <IResult> CreateBiometrics(
    [FromBody] IngestBiometricsCommand command,
    CancellationToken cancellationToken,
    IMediator mediator)
    {
         var result = await mediator.Send(command,cancellationToken);

             return result.Match(
            (_ )=>Results.Ok(new ApiResponse<string> { IsSuccess = true, Message = "Biometrics saved." }),
            (errors)=>errors.ToProblem()
         );
    }   


}
