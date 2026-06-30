using AuthenticationService.Entities;

namespace AuthenticationService.Services.Jwt
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user);
    }
}
