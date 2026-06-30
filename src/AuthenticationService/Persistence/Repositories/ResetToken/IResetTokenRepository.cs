using Microsoft.EntityFrameworkCore;
using ResetTokenEntity = AuthenticationService.Entities.ResetToken;

namespace AuthenticationService.Persistence.Repositories.ResetToken
{
    public interface IResetTokenRepository
    {
        void Add(ResetTokenEntity resetToken);

        Task<ResetTokenEntity?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken = default);
    }
}
