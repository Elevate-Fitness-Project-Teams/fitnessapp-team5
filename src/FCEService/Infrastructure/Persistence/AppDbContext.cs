using FCEService.Domain.Common;
using FCEService.Domain.Entities.CalculatedMetrics;
using FCEService.Domain.Entities.FitnessPlanConfig;
using FCEService.Domain.Entities.UserAssignedPlan;
using FCEService.Domain.Entities.UserFitnessStats;
using FCEService.Domain.Entities.UserPlanHistory;
using FCEService.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FCEService.Infrastructure.Persistence
{
    public class AppDbContext : DbContext ,IAppDbContext
    {
        private readonly IPublisher _publisher;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            IPublisher publisher)
            : base(options)
        {
            _publisher = publisher;
        }

        public DbSet<UserFitnessStats> UserFitnessStats => Set<UserFitnessStats>();
        public DbSet<CalculatedMetrics> CalculatedMetrics => Set<CalculatedMetrics>();
        public DbSet<FitnessPlanConfig> FitnessPlanConfigs => Set<FitnessPlanConfig>();
        public DbSet<UserAssignedPlan> UserAssignedPlans => Set<UserAssignedPlan>();
        public DbSet<UserPlanHistory> UserPlanHistories => Set<UserPlanHistory>();


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
             await DispatchDomainEventsAsync(cancellationToken);
             return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
             base.OnModelCreating(builder);
             builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
        {
             var domainEntities = ChangeTracker.Entries()
                 .Where(e => e.Entity is Entity baseEntity && baseEntity.DomainEvents.Count != 0)
                 .Select(e => (Entity)e.Entity)
                 .ToList();

             var domainEvents = domainEntities
                 .SelectMany(e => e.DomainEvents)
                 .ToList();

             foreach (var domainEvent in domainEvents)
             {
                 await _publisher.Publish(domainEvent, cancellationToken);
             }

             foreach (var entity in domainEntities)
             {
                 entity.ClearDomainEvents();
             }
        }
    }
}
