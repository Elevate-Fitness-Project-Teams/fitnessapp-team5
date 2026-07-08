using Microsoft.EntityFrameworkCore;
using NotificationService.InterFaces.Repositories;
using NotificationService.Persistence;

namespace NotificationService.Implementations.Repositories;

public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T> where T : class
{
    private readonly DbSet<T> _dbSet = context.Set<T>();
    public IQueryable<T> GetQueryable() => _dbSet.AsQueryable();
}
