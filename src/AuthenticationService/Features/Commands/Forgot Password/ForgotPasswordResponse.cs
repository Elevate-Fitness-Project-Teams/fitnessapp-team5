namespace AuthenticationService.Features.Forgot_Password
{
    public sealed record ForgotPasswordResponse(
        int OtpExpiresIn,
        int CanResendIn);

}
