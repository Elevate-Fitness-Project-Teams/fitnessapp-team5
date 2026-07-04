using Microsoft.EntityFrameworkCore;
using UserProfileService.Interfaces.Repositories;
using UserProfileService.Persistence;

namespace UserProfileService.Implementation.Repositories;

public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T> where T : class
{
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public IQueryable<T> GetQueryable() => _dbSet;

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default) =>
        await _dbSet.AddAsync(entity, cancellationToken);

    public void PartialUpdate(T entity, IEnumerable<string> propertyNames)
    {
        var entry = context.Entry(entity);

        if (entry.State is EntityState.Detached)
            entry = _dbSet.Attach(entity);

        foreach (var expr in propertyNames)
            entry.Property(expr).IsModified = true;
    }
}
