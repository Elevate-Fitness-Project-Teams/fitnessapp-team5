using System.Security.Claims;

namespace SmartCoachService.Services.CurrentUser
{
    public sealed class CurrentUserService(IHttpContextAccessor _httpContextAccessor)
        : ICurrentUserService
    {

        public Guid UserId => 
            Guid.Parse(_httpContextAccessor.HttpContext!.User
                       .FindFirstValue(ClaimTypes.NameIdentifier)!);

        public string? Email =>
            _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Email);

        public string? Role =>
            _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Role);

        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext!.User?.Identity?.IsAuthenticated ?? false;
    }
}
