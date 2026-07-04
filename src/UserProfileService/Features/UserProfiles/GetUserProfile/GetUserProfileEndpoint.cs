using Carter;
using MediatR;
using UserProfileService.Contracts.UserProfiles;
using UserProfileService.Extensions;

namespace UserProfileService.Features.UserProfiles.GetUserProfile;

public class GetUserProfileEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/me", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetUserProfileQuery(), cancellationToken);

            return result.ToHandleResult();
        })
         .WithName("GetUserProfile")
         .WithTags("UserProfile")
         .RequireAuthorization()
         .Produces<UserProfileResponse>(StatusCodes.Status200OK)
         .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
