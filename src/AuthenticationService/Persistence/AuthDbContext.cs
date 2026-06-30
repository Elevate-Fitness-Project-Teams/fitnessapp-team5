using AuthenticationService.Entities;
using Microsoft.EntityFrameworkCore;
using ResetTokenEntity = AuthenticationService.Entities.ResetToken;
using RefreshTokenEntity = AuthenticationService.Entities.RefreshToken;

namespace AuthenticationService.Persistence
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
        public DbSet<LoginAttempt> LoginAttempts { get; set; }
        public DbSet<OtpCode> OtpCodes { get; set; }
        public DbSet<ResetTokenEntity> ResetTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);
        }
    }
}