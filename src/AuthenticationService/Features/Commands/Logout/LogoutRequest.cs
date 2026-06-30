using AuthenticationService.Entities;

namespace AuthenticationService.Features.Commands.Logout
{
    public sealed record LogoutRequest(string RefreshToken);
}
