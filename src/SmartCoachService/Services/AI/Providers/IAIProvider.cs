namespace SmartCoachService.Services.AI.Providers
{
    public interface IAIProvider
    {
        string Name { get; }

        Task<AIResponse> GenerateReplyAsync(
            AIRequest request,
            CancellationToken cancellationToken = default);
    }
}
