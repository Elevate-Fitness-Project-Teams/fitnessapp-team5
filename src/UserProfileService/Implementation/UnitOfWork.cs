using UserProfileService.Implementation.Repositories;
using UserProfileService.Interfaces;
using UserProfileService.Interfaces.Repositories;
using UserProfileService.Persistence;

namespace UserProfileService.Implementation;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private readonly Dictionary<Type, object> _repositories = [];

    public IGenericRepository<T> Repository<T>() where T : class
    {
        var type = typeof(T);

        if (_repositories.TryGetValue(type, out var repository))
            return (IGenericRepository<T>)repository;

        var newRepo = new GenericRepository<T>(context);

        _repositories.Add(type, newRepo);

        return newRepo;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await context.SaveChangesAsync(cancellationToken);
}
