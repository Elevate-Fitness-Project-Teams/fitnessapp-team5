namespace SmartCoachService.Services.AI.Providers
{
    public sealed class ClaudeProvider : IAIProvider
    {
        public string Name => "Claude";

        public Task<AIResponse> GenerateReplyAsync(
            AIRequest request,
            CancellationToken cancellationToken = default)
        {
            var response = new AIResponse(
                Reply: $"Claude Response: {request.UserMessage}",

                FollowUpSuggestions:
                [
                    "Explain more",
                "Recommend a workout",
                "Recommend a meal"
                ]);

            return Task.FromResult(response);
        }
    }
}
