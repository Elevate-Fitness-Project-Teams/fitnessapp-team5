using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace AuthenticationService.Features.Commands.Login
{
    public static class LoginCommandEndpoint
    {
        public static IEndpointRouteBuilder MapLoginEndpoint(
            this IEndpointRouteBuilder app)
        {

            app.MapPost("/api/v1/auth/login", [AllowAnonymous] async (
                                              LoginRequest request,
                                              IMediator mediator,
                                              CancellationToken cancellationToken) =>
            {
                var command = new LoginCommand(
                    request.Email,
                    request.Password);

                var result = await mediator.Send(command, cancellationToken);

                return result;
            })
                .WithName("Login")
                .WithSummary("Login")
                .WithDescription("Authenticates a user and returns JWT tokens.")
                .WithTags("Authentication");


            return app;
        }
    }
}
