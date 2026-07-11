using System.Security.Claims;

namespace NutritionService.Infrastructure.Authentication
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
    }

    public sealed class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? UserId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                var value = user?.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? user?.FindFirstValue("sub")
                    ?? user?.FindFirstValue("userId");

                return Guid.TryParse(value, out var userId) ? userId : null;
            }
        }
    }
}
