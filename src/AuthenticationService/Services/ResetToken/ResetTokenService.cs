using System.Security.Cryptography;
using System.Text;

namespace AuthenticationService.Services.ResetToken
{
    public sealed class ResetTokenService : IResetTokenService
    {
        public string GenerateToken()
        {
            return Convert.ToBase64String(
                RandomNumberGenerator.GetBytes(64));
        }

        public string Hash(string token)
        {
            var bytes = SHA256.HashData(
                Encoding.UTF8.GetBytes(token));

            return Convert.ToHexString(bytes);
        }
    }
}
