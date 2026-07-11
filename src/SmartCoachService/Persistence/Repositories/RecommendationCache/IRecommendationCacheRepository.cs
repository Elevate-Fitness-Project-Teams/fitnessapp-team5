using RecommendationCacheEntity = SmartCoachService.Entities.RecommendationCache;

namespace SmartCoachService.Persistence.Repositories.RecommendationCache
{
    public interface IRecommendationCacheRepository
    {
        Task<RecommendationCacheEntity?> GetCacheByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        void Add(RecommendationCacheEntity recommendationCache);

    }
}
