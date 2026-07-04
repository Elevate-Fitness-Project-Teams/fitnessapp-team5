namespace UserProfileService.Interfaces.Repositories;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T> GetQueryable();
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void PartialUpdate(T entity, IEnumerable<string> propertyNames);
}
