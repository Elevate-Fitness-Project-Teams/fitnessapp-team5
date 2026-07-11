using Azure;

namespace SmartCoachService.Services.AI
{
    public interface IAIService
    {
        Task<AIResponse> GenerateReplyAsync(
            AIRequest request,
            CancellationToken cancellationToken = default);
    }
}
