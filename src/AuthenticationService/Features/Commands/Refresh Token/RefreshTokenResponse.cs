namespace AuthenticationService.Features.Refresh_Token
{
    public sealed record RefreshTokenResponse(string AccessToken, 
                                             string RefreshToken);
}
