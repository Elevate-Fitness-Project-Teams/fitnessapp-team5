using Microsoft.EntityFrameworkCore;
using RecommendationCacheEntity = SmartCoachService.Entities.RecommendationCache;
namespace SmartCoachService.Persistence.Repositories.RecommendationCache
{
    public sealed class RecommendationCacheRepository(SmartCoachDbContext _context)
         : IRecommendationCacheRepository
    {
        public async Task<RecommendationCacheEntity?> GetCacheByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await _context.RecommendationCaches.FirstOrDefaultAsync(
                                                         x => x.UserId == userId,
                                                       cancellationToken);
        }

        public void Add(RecommendationCacheEntity recommendationCache)
        {
            _context.RecommendationCaches.Add(recommendationCache);
        }
    }
}
