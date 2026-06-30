using MediatR;

namespace AuthenticationService.Features.Verify_OTP
{
    public static class VerifyOTPCommandEndpoint
    {
        public static IEndpointRouteBuilder MapVerifyOTPEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v1/auth/verify-otp", async (VerifyOTPRequest request, 
                                                     IMediator mediator, 
                                                     CancellationToken cancellationToken) =>
            {
                var command = new VerifyOTPCommand(request.Email, request.OTP);
                var result = await mediator.Send(command, cancellationToken);
                return result;
            })
            .WithName("VerifyOTP")
            .WithTags("Authentication")
            .WithSummary("Verify")
            .WithDescription("Verify a one-time password for a user");

            return app;
        }
    }
}
