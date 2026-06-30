namespace AuthenticationService.Services.Email
{
    public interface IEmailService
    {
        Task SendOtpAsync(string email, string otp, CancellationToken cancellationToken = default);
    }
}
