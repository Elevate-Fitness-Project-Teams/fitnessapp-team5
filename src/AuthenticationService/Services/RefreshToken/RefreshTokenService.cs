using System.Security.Cryptography;
using AuthenticationService.Entities;
using RefreshTokenEntity = AuthenticationService.Entities.RefreshToken;
namespace AuthenticationService.Services.RefreshToken
{
    public sealed class RefreshTokenService : IRefreshTokenService
    {
        public RefreshTokenEntity Generate(User user)
        {
            return new RefreshTokenEntity
            {
                Id = Guid.CreateVersion7(),
                UserId = user.Id,
                Token = Convert.ToBase64String(
                    RandomNumberGenerator.GetBytes(64)),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30)
            };
        }
    }
}
