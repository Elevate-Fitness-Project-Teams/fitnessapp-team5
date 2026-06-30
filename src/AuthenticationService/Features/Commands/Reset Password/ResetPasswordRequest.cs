namespace AuthenticationService.Features.Commands.Reset_Password
{
    public sealed record ResetPasswordRequest(string ResetToken,string NewPassword, string ConfirmedPassword);
}
