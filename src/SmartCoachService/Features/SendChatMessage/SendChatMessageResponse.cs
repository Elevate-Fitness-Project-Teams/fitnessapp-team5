namespace SmartCoachService.Features.SendChatMessage
{
    public sealed record SendChatMessageResponse(Guid SessionId,
                                                 string Reply,
                                                 List<string> FollowUpSuggestions);
}
