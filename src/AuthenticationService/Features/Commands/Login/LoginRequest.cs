namespace AuthenticationService.Features.Commands.Login
{
    public sealed record LoginRequest(string Email, string Password);
}
