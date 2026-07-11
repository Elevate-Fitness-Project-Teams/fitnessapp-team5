namespace SmartCoachService.Features.GetChatHistory.Dtos
{
    public sealed record ChatSessionDto(Guid Id,
                                        string Title,
                                        DateTime CreatedAt);
}
