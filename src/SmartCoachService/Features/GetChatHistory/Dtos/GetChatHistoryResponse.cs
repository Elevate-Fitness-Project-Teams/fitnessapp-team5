namespace SmartCoachService.Features.GetChatHistory.Dtos
{
    public sealed record GetChatHistoryResponse(IReadOnlyList<ChatSessionDto>? Sessions,
                                                IReadOnlyList<ChatMessageDto>? Messages);
}
 