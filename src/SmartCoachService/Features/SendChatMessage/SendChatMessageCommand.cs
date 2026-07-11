using MediatR;
using SmartCoachService.Common;

namespace SmartCoachService.Features.SendChatMessage
{
    public sealed record SendChatMessageCommand(string Message, Guid? SessionId)
    : IRequest<RequestResult<SendChatMessageResponse>>;
}
