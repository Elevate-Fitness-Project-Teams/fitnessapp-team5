namespace AuthenticationService.Services.OtpCode
{
    public interface IOtpCodeService
    {
        string GenerateCode();
        string Hash(string code);

        bool Verify(string otp, string hashedOtp);
    }
}
