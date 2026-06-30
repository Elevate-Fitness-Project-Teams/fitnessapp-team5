using AuthenticationService.Features.Commands.Login;
using AuthenticationService.Features.Commands.Logout;
using AuthenticationService.Features.Commands.Register;
using AuthenticationService.Features.Forgot_Password;
using AuthenticationService.Features.Refresh_Token;
using AuthenticationService.Features.Reset_Password;
using AuthenticationService.Features.Verify_OTP;

namespace AuthenticationService
{
    public static class AllEndpoints
    {
        public static IEndpointRouteBuilder MapAuthenticationEndpoints(
          this IEndpointRouteBuilder app)
        {
            app.MapRegisterEndpoint();
            app.MapLoginEndpoint();
            app.MapRefreshTokenEndpoint();
            app.MapLogoutEndpoint();
            app.MapForgotPasswordEndpoint();
            app.MapVerifyOTPEndpoint();
            app.MapResetPasswordEndpoint();

            return app;
        }
    }
}
