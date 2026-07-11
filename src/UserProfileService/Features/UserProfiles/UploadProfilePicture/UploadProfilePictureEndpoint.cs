using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserProfileService.Contracts.UserProfiles;
using UserProfileService.Extensions;

namespace UserProfileService.Features.UserProfiles.UploadProfilePicture;

public class UploadProfilePictureEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/me/picture", async ([FromForm] UploadPictureRequest request,
            ISender sender, CancellationToken cancellationToken) =>
        {
            var command = request.Adapt<UploadProfilePictureCommand>();

            var result = await sender.Send(command, cancellationToken);

            return result.ToHandleResult();
        })
        .WithName("UserPicture")
        .WithTags("UserProfile")
        .RequireAuthorization()
        .DisableAntiforgery()
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
