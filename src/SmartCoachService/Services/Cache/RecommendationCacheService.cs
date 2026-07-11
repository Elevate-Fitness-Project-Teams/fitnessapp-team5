using SmartCoachService.Entities;
using SmartCoachService.Persistence.Repositories.RecommendationCache;
using SmartCoachService.Persistence.Repositories.UnitOfWork;
using SmartCoachService.Services.CurrentUser;

namespace SmartCoachService.Services.Cache
{
    public sealed class RecommendationCacheService(
                     IRecommendationCacheRepository recommendationCacheRepo,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork)
    : IRecommendationCacheService
    {
        public async Task<string?> GetUserContextAsync(
            CancellationToken cancellationToken = default)
        {
            var cache = await recommendationCacheRepo.GetCacheByUserIdAsync(
                currentUser.UserId,
                cancellationToken);

            if (cache is null || cache.IsExpired())
                return null;

            return cache.UserContextJson;
        }

        public async Task SetUserContextAsync(
            string userContextJson,
            string homeFeedJson,
            CancellationToken cancellationToken = default)
        {
            var cache = await recommendationCacheRepo.GetCacheByUserIdAsync(
                currentUser.UserId,
                cancellationToken);

            if (cache is null)
            {
                recommendationCacheRepo.Add(
                    new RecommendationCache(
                        currentUser.UserId,
                        userContextJson,
                        homeFeedJson,
                        DateTime.UtcNow.AddMinutes(30)));
            }
            else
            {
                cache.Update(
                    userContextJson,
                    homeFeedJson,
                    DateTime.UtcNow.AddMinutes(30));
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
