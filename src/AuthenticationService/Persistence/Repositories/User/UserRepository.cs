using AuthenticationService.Entities;
using Microsoft.EntityFrameworkCore;
using UserEntity = AuthenticationService.Entities.User;

namespace AuthenticationService.Persistence.Repositories.User
{
    public class UserRepository(AuthDbContext _context) : IUserRepository
    {
        public void Add(UserEntity user)
        {
            _context.Users.Add(user);
        }

        public void Update(UserEntity user)
        {
            _context.Users.Update(user);
        }

        public async Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<UserEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
