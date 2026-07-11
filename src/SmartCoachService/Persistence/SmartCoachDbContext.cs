using Microsoft.EntityFrameworkCore;
using SmartCoachService.Entities;

namespace SmartCoachService.Persistence
{
    public sealed class SmartCoachDbContext : DbContext
    {
        public DbSet<ChatSession> ChatSessions => Set<ChatSession>();
        public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
        public DbSet<RecommendationCache> RecommendationCaches => Set<RecommendationCache>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(SmartCoachDbContext).Assembly);
        }
    }
}