namespace SmartCoachService.Services.AI.Providers
{
    public sealed class OpenAIProvider : IAIProvider
    {
        public string Name => "OpenAI";

        public Task<AIResponse> GenerateReplyAsync(
            AIRequest request,
            CancellationToken cancellationToken = default)
        {
            var response = new AIResponse(
                Reply: $"OpenAI Response: {request.UserMessage}",

                FollowUpSuggestions:
                [
                    "Show today's workout",
                    "Show today's meals",
                    "View my progress"
                ]);

            return Task.FromResult(response);
        }
    }
}
