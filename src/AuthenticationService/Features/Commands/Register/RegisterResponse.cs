namespace AuthenticationService.Features.Commands.Register
{
    public sealed record RegisterResponse(
                  Guid UserId,
                  bool RequiresProfileCompletion);
}
