using AuthenticationService.Common;
using MediatR;

namespace AuthenticationService.Features.Commands.Logout
{
    public sealed record LogoutCommand(string RefreshToken)
        : IRequest<RequestResult<LogoutResponse>>;
}
