namespace SmartCoachService.Services.ChatRateLimit
{
    public interface IChatRateLimitService
    {
        Task<bool> CanSendMessageAsync(Guid userId,
                                       CancellationToken cancellationToken = default);

        Task IncrementAsync(Guid userId,
                            CancellationToken cancellationToken = default);
    }
}
