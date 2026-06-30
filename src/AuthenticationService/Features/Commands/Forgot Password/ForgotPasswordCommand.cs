using AuthenticationService.Common;
using MediatR;

namespace AuthenticationService.Features.Forgot_Password
{
    public sealed record ForgotPasswordCommand(string Email)
        : IRequest<RequestResult<ForgotPasswordResponse>>;
}
