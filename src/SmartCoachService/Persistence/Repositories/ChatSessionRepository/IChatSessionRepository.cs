using SmartCoachService.Entities;

namespace SmartCoachService.Persistence.Repositories.ChatSessionRepository
{
    public interface IChatSessionRepository
    {
        void Add(ChatSession chatSession);

        Task<ChatSession?> GetByIdAsync(
            Guid sessionId,
            CancellationToken cancellationToken = default);

        IQueryable<ChatSession> GetPagedByUserId(
            Guid userId,
            int page,
            int pageSize);

        Task<int> CountByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
    }
}
