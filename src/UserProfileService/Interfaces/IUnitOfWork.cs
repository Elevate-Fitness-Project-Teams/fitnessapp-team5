using UserProfileService.Interfaces.Repositories;

namespace UserProfileService.Interfaces;

public interface IUnitOfWork
{
    public IGenericRepository<T> Repository<T>() where T : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
