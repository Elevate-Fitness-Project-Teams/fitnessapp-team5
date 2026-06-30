namespace AuthenticationService.Services.Password
{
    public interface IPasswordService
    {
        string Hash(string password);
        bool Verify(string password, string passwordHash);
    }
}
