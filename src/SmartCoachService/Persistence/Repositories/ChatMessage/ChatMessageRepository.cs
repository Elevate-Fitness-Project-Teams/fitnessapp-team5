using SmartCoachService.Entities;
using SmartCoachService.Persistence.Repositories.ChatSessionRepository;

namespace SmartCoachService.Persistence.Repositories.ChatMessageRepository
{
    public sealed class ChatMessageRepository(SmartCoachDbContext _context)
        : IChatMessageRepository
    {
        public void Add(ChatMessage message)
        {
            _context.ChatMessages.Add(message);
        }

        public IQueryable<ChatMessage> GetBySessionId(Guid sessionId)
        {
            return _context.ChatMessages.Where(x => x.ChatSessionId == sessionId)
                                        .OrderBy(x => x.CreatedAt);
        }
    }
}

