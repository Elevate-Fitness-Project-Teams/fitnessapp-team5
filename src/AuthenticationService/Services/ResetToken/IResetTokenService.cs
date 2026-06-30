namespace AuthenticationService.Services.ResetToken
{
    public interface IResetTokenService
    {
        string GenerateToken();
        string Hash(string token);
    }
}
