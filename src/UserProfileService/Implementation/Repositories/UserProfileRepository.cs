using Microsoft.EntityFrameworkCore;
using UserProfileService.Interfaces.Repositories;
using UserProfileService.Persistence;

namespace UserProfileService.Impelementation.Repositories;

public class UserProfileRepository<T>(ApplicationDbContext context) : IUserProfileRepository<T> where T : class
{
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public IQueryable<T> GetQueryable() => _dbSet;

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default) =>
        await _dbSet.AddAsync(entity, cancellationToken);
}
