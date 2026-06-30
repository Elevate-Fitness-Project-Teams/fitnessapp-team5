namespace AuthenticationService.Persistence.Repositories.UnitOfWork
{
    public sealed class UnitOfWork(AuthDbContext context)
        : IUnitOfWork
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return context.SaveChangesAsync(cancellationToken);
        }
    }
}
