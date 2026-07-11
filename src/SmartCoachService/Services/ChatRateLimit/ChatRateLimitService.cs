using Microsoft.Extensions.Caching.Distributed;

namespace SmartCoachService.Services.ChatRateLimit
{
    public sealed class ChatRateLimitService(IDistributedCache _cache)
                  : IChatRateLimitService
{
    private const int MaxMessagesPerDay = 5;

    public async Task<bool> CanSendMessageAsync(Guid userId,
                                                CancellationToken cancellationToken = default)
    {
        var key = GetKey(userId);

        var value = await _cache.GetStringAsync(key, cancellationToken);

        if (string.IsNullOrWhiteSpace(value))
            return true;

        return int.Parse(value) < MaxMessagesPerDay;
    }

    public async Task IncrementAsync(Guid userId,
                                     CancellationToken cancellationToken = default)
    {
        var key = GetKey(userId);

        var value = await _cache.GetStringAsync(key, cancellationToken);

        var count = string.IsNullOrWhiteSpace(value) ? 1 : int.Parse(value) + 1;

        await _cache.SetStringAsync(key, count.ToString(),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
            },
            cancellationToken);
    }

    private static string GetKey(Guid userId)
                        => $"chat-limit:{userId}";
}
}
