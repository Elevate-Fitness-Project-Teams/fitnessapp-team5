namespace AuthenticationService.Features.Commands.Register
{
    public sealed record RegisterRequest(
                                  string FirstName,
                                  string LastName,
                                  string Email,
                                  string Password,
                                  string PhoneNumber
                                  );
}
