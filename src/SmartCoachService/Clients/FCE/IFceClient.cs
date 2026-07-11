namespace SmartCoachService.Clients.FCE
{
    public interface IFceClient
    {
        Task<FceResponse?> GetUserContextAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
