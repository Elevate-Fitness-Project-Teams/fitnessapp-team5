using SmartCoachService.Entities;

namespace SmartCoachService.Services.Cache
{
    public interface IRecommendationCacheService
    {
        Task<string?> GetUserContextAsync(
            CancellationToken cancellationToken = default);

        Task SetUserContextAsync(
            string userContextJson,
            string homeFeedJson,
            CancellationToken cancellationToken = default);
    }
}
