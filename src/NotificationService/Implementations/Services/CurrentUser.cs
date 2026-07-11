using NotificationService.InterFaces.Services;
using System.Security.Claims;

namespace NotificationService.Implementations.Services;

public class CurrentUser(HttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext!.User;
    public Guid Id => Guid.TryParse(
        User?.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
            ? id
            : throw new UnauthorizedAccessException("User is not authenticated.");
}
