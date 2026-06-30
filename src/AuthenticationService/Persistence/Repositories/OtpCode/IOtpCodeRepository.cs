using OtpCodeEntity = AuthenticationService.Entities.OtpCode;

namespace AuthenticationService.Persistence.Repositories.OtpCode
{
    public interface IOtpCodeRepository
    {
        void Add(OtpCodeEntity otpCode);
        Task<OtpCodeEntity?> GetLatestByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<OtpCodeEntity?> GetLatestUnusedByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
