using MediatR;

namespace AuthenticationService.Features.Forgot_Password
{
    public static class ForgotPasswordCommandEndpoint
    {
        public static IEndpointRouteBuilder MapForgotPasswordEndpoint(
            this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v1/forgot-password", async (ForgotPasswordRequest request, 
                                                          IMediator mediator) =>
            {
                var command = new ForgotPasswordCommand(request.Email);
                var result = await mediator.Send(command);
                return result;
            })
            .WithName("ForgotPassword")
            .WithTags("Authentication")
            .WithSummary("Forgot Password")
            .WithDescription("Request a password reset link to be sent to the user's email address");
            
            return app;
        }
    }
}
