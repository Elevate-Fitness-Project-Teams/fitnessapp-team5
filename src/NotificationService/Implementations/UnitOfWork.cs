using NotificationService.Implementations.Repositories;
using NotificationService.InterFaces;
using NotificationService.InterFaces.Repositories;
using NotificationService.Persistence;

namespace NotificationService.Implementations;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private readonly Dictionary<Type, object> _repositories = [];
    public IGenericRepository<T> Repository<T>() where T : class
    {
        var type = typeof(T);
        if (_repositories.TryGetValue(type, out var repository))
            return (IGenericRepository<T>)repository;

        var newRepository = new GenericRepository<T>(context);

        _repositories.Add(type, newRepository);

        return newRepository;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        context.SaveChangesAsync(cancellationToken);
}
