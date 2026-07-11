using Carter;
using MediatR;
using NotificationService.Abstractions;

namespace NotificationService.Features.Notifications.GetNotifications;

public class GetNotificationsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/notifications", async ([AsParameters] RequestFilters filters, ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetNotificationsQuery(filters), cancellationToken);

            return Results.Ok(result);
        })
        .WithName("GetNotifications")
        .WithTags("Notifications")
        .RequireAuthorization()
        .Produces(StatusCodes.Status200OK)
        //.ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}