namespace SmartCoachService.Persistence.Repositories.UnitOfWork
{
    public sealed class UnitOfWork(SmartCoachDbContext _context)
        : IUnitOfWork
    {
        public async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
