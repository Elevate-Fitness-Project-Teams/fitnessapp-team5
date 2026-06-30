using AuthenticationService.Common;
using MediatR;

namespace AuthenticationService.Features.Commands.Register
{
    public sealed record RegisterCommand(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string PhoneNumber)
        : IRequest<RequestResult<RegisterResponse>>;
}
