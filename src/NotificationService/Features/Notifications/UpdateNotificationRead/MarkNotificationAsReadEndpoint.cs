using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Extensions;

namespace NotificationService.Features.Notifications.UpdateNotificationRead;

public class MarkNotificationAsReadEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/notifications/{id:int}/read", async ([FromRoute] int id, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new MarkNotificationAsReadCommand(id), cancellationToken);

            return result.ToHandleResult();
        })
        .WithName("MarkNotificationAsRead")
        .WithTags("Notifications")
        .RequireAuthorization()
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}
