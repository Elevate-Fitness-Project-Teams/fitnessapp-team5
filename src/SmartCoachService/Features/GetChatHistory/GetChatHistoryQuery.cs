using MediatR;
using SmartCoachService.Common;
using SmartCoachService.Features.GetChatHistory.Dtos;

namespace SmartCoachService.Features.GetChatHistory
{
    public sealed record GetChatHistoryQuery(int Page,
                                             int PageSize,
                                             Guid? SessionId)
        : IRequest<RequestResult<GetChatHistoryResponse>>;
}
