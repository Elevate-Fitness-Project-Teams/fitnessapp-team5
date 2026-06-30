namespace AuthenticationService.Features.Commands.Login
{
    public sealed record LoginResponse(
        string AccessToken,
        string RefreshToken,
        bool ProfileCompleted,
        bool IsPremium);
}
