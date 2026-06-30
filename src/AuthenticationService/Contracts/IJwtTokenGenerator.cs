using AuthenticationService.Entities;

namespace AuthenticationService.Abstractions
{
    public interface IJwtTokenGenerator
    {
        string Generate(User user);

    }
}
