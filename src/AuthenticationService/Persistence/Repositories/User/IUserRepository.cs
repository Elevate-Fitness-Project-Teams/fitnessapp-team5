using Microsoft.EntityFrameworkCore.Diagnostics;
using UserEntity = AuthenticationService.Entities.User;

namespace AuthenticationService.Persistence.Repositories.User
{
    public interface IUserRepository
    {
        Task<UserEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        void Add(UserEntity user);

        void Update(UserEntity user);
    }
}
