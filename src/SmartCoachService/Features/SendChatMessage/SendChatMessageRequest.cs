using SmartCoachService.Entities.enums;

namespace SmartCoachService.Features.SendChatMessage
{
    public sealed record SendChatMessageRequest(string Message, Guid? SessionId);
}
