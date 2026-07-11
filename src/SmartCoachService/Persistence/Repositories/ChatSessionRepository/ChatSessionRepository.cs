using System.Collections;
using Microsoft.EntityFrameworkCore;
using SmartCoachService.Entities;

namespace SmartCoachService.Persistence.Repositories.ChatSessionRepository
{
    public class ChatSessionRepository(SmartCoachDbContext _context) 
        : IChatSessionRepository
    {
        public void Add(ChatSession chatSession)
        {
            _context.ChatSessions.Add(chatSession);
        }

        public async Task<ChatSession?> GetByIdAsync(
            Guid sessionId,
            CancellationToken cancellationToken = default)
        {
            return await _context.ChatSessions.Include(x => x.Messages)
                                              .FirstOrDefaultAsync(
                                                  x => x.Id == sessionId,
                                                  cancellationToken);
        }

        public IQueryable<ChatSession> GetPagedByUserId(
            Guid userId,
            int page,
            int pageSize)
        {
            return  _context.ChatSessions.Where(x => x.UserId == userId)
                                         .OrderByDescending(x => x.CreatedAt)
                                         .Skip((page - 1) * pageSize)
                                         .Take(pageSize);
        }

        public async Task<int> CountByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await _context.ChatSessions.CountAsync(x => x.UserId == userId, 
                                                         cancellationToken);
        }
    }
}
