using AuthenticationService.Common;
using MediatR;

namespace AuthenticationService.Features.Refresh_Token
{
    public sealed record RefreshTokenCommand(string RefreshToken)
        : IRequest<RequestResult<RefreshTokenResponse>>;
}
