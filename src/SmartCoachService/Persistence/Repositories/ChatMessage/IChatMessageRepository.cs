using SmartCoachService.Entities;
using SmartCoachService.Features.GetChatHistory;

namespace SmartCoachService.Persistence.Repositories.ChatMessageRepository
{
    public interface IChatMessageRepository
    {
        void Add(ChatMessage message);
        IQueryable<ChatMessage> GetBySessionId(
            Guid sessionId);
    }
}
