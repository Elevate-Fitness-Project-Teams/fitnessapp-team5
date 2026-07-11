using Carter;
using MediatR;
using UserProfileService.Extensions;

namespace UserProfileService.Features.UserSettings.GetUserSetting;

public class GetUserSettingEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/me/settings", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetUserSettingQuery(), cancellationToken);

            return result.ToHandleResult();
        })
       .WithName("UserSetting")
       .WithTags("UserProfile")
       .RequireAuthorization()
       .Produces(StatusCodes.Status200OK)
       .ProducesValidationProblem()
       .ProducesProblem(StatusCodes.Status401Unauthorized)
       .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
