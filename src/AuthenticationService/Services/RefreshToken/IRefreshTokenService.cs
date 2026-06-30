using AuthenticationService.Entities;
using RefreshTokenEntity = AuthenticationService.Entities.RefreshToken;
namespace AuthenticationService.Services.RefreshToken
{
    public interface IRefreshTokenService
    {
        RefreshTokenEntity Generate(User user);
    }
}
