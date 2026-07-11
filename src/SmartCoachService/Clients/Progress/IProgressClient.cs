namespace SmartCoachService.Clients.Progress
{
    public interface IProgressClient
    {
        Task<ProgressResponse?> GetUserProgressAsync(Guid userId,
                                                     CancellationToken cancellationToken = default);
    }
}
