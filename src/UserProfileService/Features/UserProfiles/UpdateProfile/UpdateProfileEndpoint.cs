using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserProfileService.Extensions;

namespace UserProfileService.Features.UserProfiles.UpdateProfile;

public class UpdateProfileEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v1/profile", async ([FromBody] UpdateProfileCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(command, cancellationToken);
            result.ToResult();
        })
        .WithName("UpdateProfile")
        .WithTags("UserProfile")
        .RequireAuthorization()
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status409Conflict);
    }
}
