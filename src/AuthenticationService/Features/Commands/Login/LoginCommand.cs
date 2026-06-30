using AuthenticationService.Common;
using MediatR;

namespace AuthenticationService.Features.Commands.Login
{
    public sealed record LoginCommand(string Email, string Password)
        : IRequest<RequestResult<LoginResponse>>;

}
