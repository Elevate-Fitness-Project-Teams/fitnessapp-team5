using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Features.Commands.Logout
{
    public static class LogoutCommandEndpoint
    {
        public static IEndpointRouteBuilder MapLogoutEndpoint(
            this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v1/auth/logout", [Authorize] async (
                                              [FromBody] LogoutRequest request,
                                              IMediator mediator,
                                              CancellationToken cancellationToken) =>
            {
                var command = new LogoutCommand(request.RefreshToken);
                var result = await mediator.Send(command, cancellationToken);
                return result;
            })
                .WithName("Logout")
                .WithSummary("Logout")
                .WithDescription("Logs out a user by invalidating the refresh token.")
                .WithTags("Authentication");

            return app;
        }
    }
}
