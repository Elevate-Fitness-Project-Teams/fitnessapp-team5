using AuthenticationService.Common;
using AuthenticationService.Features.Commands.Reset_Password;
using MediatR;

namespace AuthenticationService.Features.Reset_Password
{
    public sealed record ResetPasswordCommand(string ResetToken, string NewPassword, string ConfirmedPassword) 
        : IRequest<RequestResult<ResetPasswordResponse>>;
}
