namespace AuthenticationService.Features.Verify_OTP
{
    public sealed record VerifyOTPRequest(string Email, string OTP);
}
