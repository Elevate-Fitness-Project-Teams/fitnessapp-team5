using Azure;
using SmartCoachService.Services.AI.Providers;

namespace SmartCoachService.Services.AI
{
    public sealed class AIService(
            IEnumerable<IAIProvider> providers,
            ILogger<AIService> logger)
        : IAIService
    {
        private readonly IReadOnlyList<IAIProvider> _providers = providers.ToList();

        public async Task<AIResponse> GenerateReplyAsync(
            AIRequest request,
            CancellationToken cancellationToken = default)
        {
            Exception? lastException = null;

            foreach (var provider in _providers)
            {
                try
                {
                    logger.LogInformation(
                        "Trying AI Provider {Provider}",
                        provider.GetType().Name);

                    var response = await provider.GenerateReplyAsync(
                        request,
                        cancellationToken);

                    logger.LogInformation(
                        "{Provider} succeeded",
                        provider.GetType().Name);

                    return response;
                }
                catch (Exception ex)
                {
                    lastException = ex;

                    logger.LogWarning(
                        ex,
                        "{Provider} failed. Trying next provider.",
                        provider.GetType().Name);
                }
            }

            throw new InvalidOperationException(
                "No AI provider is available.",
                lastException);
        }
    }
}
