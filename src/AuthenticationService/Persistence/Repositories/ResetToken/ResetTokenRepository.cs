using Microsoft.EntityFrameworkCore;
using ResetTokenEntity = AuthenticationService.Entities.ResetToken;

namespace AuthenticationService.Persistence.Repositories.ResetToken
{
    public class ResetTokenRepository(AuthDbContext _context)
    : IResetTokenRepository
    {
        public void Add(ResetTokenEntity resetToken)
        {
            _context.ResetTokens.Add(resetToken);
        }

        public async Task<ResetTokenEntity?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken = default)
        {
            return await _context.ResetTokens
                                 .FirstOrDefaultAsync(x => x.TokenHash == tokenHash,
                                                      cancellationToken);
        }
    }
}
