using Carter;
using MediatR;
using UserProfileService.Contracts.UserSetting;
using UserProfileService.Extensions;

namespace UserProfileService.Features.UserSettings.UpdateSetting;

public class UpdateSettingEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/me/settings", async (ISender sender, UpdateSettingRequest request, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new UpdateSettingOrchestrator
                (request.Notification, request.Privacy, request.Preferences), cancellationToken);

            return result.ToHandleResult();
        })
        .WithName("UpdateUserSetting")
        .WithTags("UserProfile")
        .RequireAuthorization()
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
