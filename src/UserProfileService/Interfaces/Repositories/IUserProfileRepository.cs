namespace UserProfileService.Interfaces.Repositories;

public interface IUserProfileRepository<T> where T : class
{
    IQueryable<T> GetQueryable();
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
}
