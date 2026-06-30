using System.Security.Claims;
using UserProfileService.Interfaces.Services;

namespace UserProfileService.Implementation.Services;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;
    public Guid Id => Guid.TryParse(
        User?.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
            ? id
            : throw new UnauthorizedAccessException("User is not authenticated.");
}
