using System.Security.Cryptography;
using System.Text;

namespace AuthenticationService.Services.OtpCode
{
    public class OtpCodeService : IOtpCodeService
    {
        public string GenerateCode()
        {
            return RandomNumberGenerator
                .GetInt32(100000, 1000000)
                .ToString();
        }

        public string Hash(string code)
        {
            var bytes = SHA256.HashData(
                Encoding.UTF8.GetBytes(code));

            return Convert.ToHexString(bytes);
        }

        public bool Verify(string otp, string hashedOtp)
        {
            var computedHash = Hash(otp);

            return computedHash.Equals(
                hashedOtp,
                StringComparison.Ordinal);
        }
    }
}
