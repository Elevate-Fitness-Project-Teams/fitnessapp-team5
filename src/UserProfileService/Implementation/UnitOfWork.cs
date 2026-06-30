using UserProfileService.Impelementation.Repositories;
using UserProfileService.Interfaces;
using UserProfileService.Interfaces.Repositories;
using UserProfileService.Persistence;

namespace UserProfileService.Implementation;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private readonly Dictionary<Type, object> _repositories = [];

    public IUserProfileRepository<T> Repository<T>() where T : class
    {
        var type = typeof(T);

        if (_repositories.TryGetValue(type, out var repository))
            return (IUserProfileRepository<T>)repository;

        var newRepo = new UserProfileRepository<T>(context);

        _repositories.Add(type, newRepo);

        return newRepo;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await context.SaveChangesAsync(cancellationToken);
}
