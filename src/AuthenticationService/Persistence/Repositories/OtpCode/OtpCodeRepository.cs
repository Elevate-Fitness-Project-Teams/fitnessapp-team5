using AuthenticationService.Entities;
using Microsoft.EntityFrameworkCore;
using OtpCodeEntity = AuthenticationService.Entities.OtpCode;

namespace AuthenticationService.Persistence.Repositories.OtpCode
{
    public class OtpCodeRepository(AuthDbContext _context) : IOtpCodeRepository
    {
        public void Add(OtpCodeEntity otpCode)
        {
            _context.OtpCodes.Add(otpCode);
        }

        public async Task<OtpCodeEntity?> GetLatestByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.OtpCodes
                                 .Where(x => x.UserId == userId)
                                 .OrderByDescending(x => x.CreatedAt)
                                 .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<OtpCodeEntity?> GetLatestUnusedByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.OtpCodes.Where(x => x.UserId == userId && !x.IsUsed)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);
        }

    }
}
