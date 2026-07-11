using MediatR;
using SmartCoachService.Common;
using SmartCoachService.Features.GetChatHistory.Dtos;
using SmartCoachService.Persistence.Repositories.ChatMessageRepository;
using SmartCoachService.Persistence.Repositories.ChatSessionRepository;
using SmartCoachService.Services.CurrentUser;

namespace SmartCoachService.Features.GetChatHistory
{
    public sealed class GetChatHistoryHandler(
                                              IChatSessionRepository _chatSessionRepo,
                                              IChatMessageRepository _chatMessageRepo,
                                              ICurrentUserService _currentUser)
        : IRequestHandler<GetChatHistoryQuery, RequestResult<GetChatHistoryResponse>>
    {
        public async Task<RequestResult<GetChatHistoryResponse>> Handle(
        GetChatHistoryQuery request,
        CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            // ==========================
            // Return Chat Sessions
            // ==========================
            if (request.SessionId is null)
            {
                var sessions = _chatSessionRepo.GetPagedByUserId(
                    userId,
                    request.Page,
                    request.PageSize);

                var response = new GetChatHistoryResponse(
                    Sessions: sessions.Select(x => new ChatSessionDto(
                                                   x.Id,
                                                   x.Title,
                                                   x.CreatedAt))
                                      .ToList(),

                    Messages: null);

                return RequestResult<GetChatHistoryResponse>.Success(response);
            }

            // ==========================
            // Return Chat Messages
            // ==========================

            var session = await _chatSessionRepo.GetByIdAsync(
                request.SessionId.Value,
                cancellationToken);

            if (session is null || session.UserId != userId)
            {
                return RequestResult<GetChatHistoryResponse>.Failure(
                    "RES_SESSION_NOT_FOUND",
                    statusCode: StatusCodes.Status404NotFound);
            }

            var messages = _chatMessageRepo.GetBySessionId(session.Id);

            var responseResult = new GetChatHistoryResponse(
                Sessions: null,

                Messages: messages.OrderBy(x => x.CreatedAt)
                                  .Select(x => new ChatMessageDto(
                                      x.Id,
                                      x.Sender,
                                      x.Message,
                                      x.CreatedAt))
                                  .ToList());

            return RequestResult<GetChatHistoryResponse>.Success(responseResult);
        }
    }
}
