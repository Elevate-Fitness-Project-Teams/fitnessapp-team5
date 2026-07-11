namespace SmartCoachService.Features.GetChatHistory
{
    public sealed record GetChatHistoryRequest(int Page = 1,
                                               int PageSize = 10,
                                               Guid? SessionId = null);
}
