using Microsoft.EntityFrameworkCore;
using RefreshTokenEntity = AuthenticationService.Entities.RefreshToken;
namespace AuthenticationService.Persistence.Repositories.RefreshToken
{
    public class RefreshTokenRepository(AuthDbContext _context)
        : IRefreshTokenRepository
    {

        public void Add(RefreshTokenEntity refreshToken)
        {
            _context.RefreshTokens.Add(refreshToken);
        }

        public async Task<RefreshTokenEntity?> GetByTokenAsync(
            string token,
            CancellationToken cancellationToken = default)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(
                    x => x.Token == token,
                    cancellationToken);
        }

        public async Task<List<RefreshTokenEntity>> GetActiveByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await _context.RefreshTokens
                .Where(x =>
                    x.UserId == userId &&
                    x.RevokedAt == null &&
                    x.ExpiresAt > DateTime.UtcNow)
                .ToListAsync(cancellationToken);
        }
    }
}
