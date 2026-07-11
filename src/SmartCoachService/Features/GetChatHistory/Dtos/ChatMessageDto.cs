using SmartCoachService.Entities.enums;

namespace SmartCoachService.Features.GetChatHistory.Dtos
{
    public sealed record ChatMessageDto(Guid Id,
                                        SenderType Sender,
                                        string Message,
                                        DateTime CreatedAt);
}
