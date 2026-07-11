using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserProfileService.Contracts.UserProfiles;
using UserProfileService.Extensions;

namespace UserProfileService.Features.UserProfiles.UpdateProfile;

public class UpdateProfileEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v1/me", async ([FromBody] UpdateProfileRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = request.Adapt<UpdateProfileCommand>();

            var result = await sender.Send(command, cancellationToken);

            return result.ToHandleResult();
        })
        .WithName("UpdateProfile")
        .WithTags("UserProfile")
        .RequireAuthorization()
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict);
    }
}
