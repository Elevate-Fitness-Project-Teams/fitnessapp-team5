using AuthenticationService.Features.Commands.Reset_Password;
using MediatR;

namespace AuthenticationService.Features.Reset_Password
{
    public static class ResetPasswordCommandEndpoint
    {
        public static IEndpointRouteBuilder MapResetPasswordEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v1/auth/reset-password", async (ResetPasswordRequest request, 
                                                              IMediator mediator,
                                                              CancellationToken cancellationToken) =>
            {
                var command = new ResetPasswordCommand(request.ResetToken, 
                                                       request.NewPassword, 
                                                       request.ConfirmedPassword);
                var result = await mediator.Send(command, cancellationToken);
                return result;
            })
            .WithName("ResetPassword")
            .WithTags("Authentication")
            .WithSummary("Reset Password")
            .WithDescription("Reset a user's password with a valid reset token");

            return app;
        }
    }
}
