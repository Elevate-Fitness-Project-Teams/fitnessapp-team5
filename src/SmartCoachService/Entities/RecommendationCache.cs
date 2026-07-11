namespace SmartCoachService.Entities
{
    public sealed class RecommendationCache
    {
        public Guid UserId { get; private set; }
        public string UserContextJson { get; private set; } = default!;
        public string HomeFeedDataJson { get; private set; } = default!;
        public DateTime CachedAt { get; private set; }
        public DateTime ExpiresAt { get; private set; }

        public RecommendationCache(Guid userId,
                                   string userContextJson,
                                   string homeFeedDataJson,
                                   DateTime expiresAt)
        {
            UserId = userId;
            UserContextJson = userContextJson;
            HomeFeedDataJson = homeFeedDataJson;

            CachedAt = DateTime.UtcNow;
            ExpiresAt = expiresAt;
        }

        public void Update(string userContextJson,
                           string homeFeedDataJson,
                           DateTime expiresAt)
        {
            UserContextJson = userContextJson;
            HomeFeedDataJson = homeFeedDataJson;

            CachedAt = DateTime.UtcNow;
            ExpiresAt = expiresAt;
        }

        public bool IsExpired()
        {
            return DateTime.UtcNow >= ExpiresAt;
        }
    }
}
