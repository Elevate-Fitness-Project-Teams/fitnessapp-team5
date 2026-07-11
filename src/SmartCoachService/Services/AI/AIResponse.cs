namespace SmartCoachService.Services.AI
{
    public sealed record AIResponse(string Reply,
                                    IReadOnlyList<string> FollowUpSuggestions);
}
