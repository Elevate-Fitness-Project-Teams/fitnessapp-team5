using System.Net.Mail;
using AuthenticationService.Common;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace AuthenticationService.Services.Email
{
    public sealed class EmailService(IOptions<EmailSettings> options)
        : IEmailService
    {
        private readonly EmailSettings _emailsettings = options.Value;

        public async Task SendOtpAsync(string email, string otp,
                                       CancellationToken cancellationToken = default)
        {
            var message = new MimeMessage();

            message.From.Add(
                MailboxAddress.Parse(_emailsettings.From));

            message.To.Add(
                MailboxAddress.Parse(email));

            message.Subject = "Password Reset OTP";

            message.Body = new TextPart("plain")
            {
                Text = $"""
                       Your password reset code is: {otp} 
                       This code expires in 10 minutes.
                       """
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            await smtp.ConnectAsync(_emailsettings.Host,
                                    _emailsettings.Port,
                                    SecureSocketOptions.StartTls,
                                    cancellationToken);

            await smtp.AuthenticateAsync(
                _emailsettings.Username,
                _emailsettings.Password,
                cancellationToken);

            await smtp.SendAsync(
                message,
                cancellationToken);

            await smtp.DisconnectAsync(
                true,
                cancellationToken);
        }
    }
}
