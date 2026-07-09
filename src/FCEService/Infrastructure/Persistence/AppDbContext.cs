using FCEService.Domain.Common;
using FCEService.Domain.Common.Results;
using FCEService.Domain.Entities.CalculatedMetrics;
using FCEService.Domain.Entities.FitnessPlanConfig;
using FCEService.Domain.Entities.UserAssignedPlan;
using FCEService.Domain.Entities.UserFitnessStats;
using FCEService.Domain.Entities.UserPlanHistory;
using FCEService.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace FCEService.Infrastructure.Persistence
{
    public sealed class AppDbContext : DbContext, IAppDbContext
    {
        private readonly IPublisher _publisher;
        private readonly ILogger<AppDbContext> _logger;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            IPublisher publisher,
            ILogger<AppDbContext> logger)
            : base(options)
        {
            _publisher = publisher;
            _logger = logger;
        }

        public DbSet<UserFitnessStats> UserFitnessStats => Set<UserFitnessStats>();
        public DbSet<CalculatedMetrics> CalculatedMetrics => Set<CalculatedMetrics>();
        public DbSet<FitnessPlanConfig> FitnessPlanConfigs => Set<FitnessPlanConfig>();
        public DbSet<UserAssignedPlan> UserAssignedPlans => Set<UserAssignedPlan>();
        public DbSet<UserPlanHistory> UserPlanHistories => Set<UserPlanHistory>();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            int totalSaved = 0;
            const int maxIterations = 5; // Guard against infinite event loops
            int iteration = 0;

            do
            {
                // Save whatever is currently tracked (main entity OR event-triggered entities)
                totalSaved += await base.SaveChangesAsync(cancellationToken);

                // Collect and dispatch ALL pending domain events from ALL tracked entities
                var hasPendingEvents = await DispatchDomainEventsAsync(cancellationToken);

                // If no new events were dispatched → no new entities were added → done
                if (!hasPendingEvents)
                    break;

                iteration++;

            } while (ChangeTracker.HasChanges() && iteration < maxIterations);

            if (iteration >= maxIterations)
                _logger.LogWarning("SaveChangesAsync hit max iteration limit ({Max}). Check for circular domain event chains.", maxIterations);

            return totalSaved;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            // No Outbox tables — FCE is Consumer Only
        }

        private async Task<bool> DispatchDomainEventsAsync(CancellationToken cancellationToken)
        {
            var domainEntities = ChangeTracker.Entries()
                .Where(e => e.Entity is Entity baseEntity && baseEntity.DomainEvents.Count != 0)
                .Select(e => (Entity)e.Entity)
                .ToList();

            if (domainEntities.Count == 0)
                return false;

            var domainEvents = domainEntities
                .SelectMany(e => e.DomainEvents)
                .ToList();

            foreach (var entity in domainEntities)
            {
                entity.ClearDomainEvents();
            }

            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent, cancellationToken);
            }

            return true;
        }

        private int _depth = 0;
        private IDbContextTransaction? _transaction;

        public async Task<Result<Success>> ExecuteAsync(Func<Task<Result<Success>>> action)
        {
            if (_depth == 0 && _transaction == null)
            {
                _transaction = await this.Database.BeginTransactionAsync();
            }
            _depth++;

            try
            {
                var result = await action();

                if (result.IsError)
                {
                    if (_depth == 1)
                    {
                        await _transaction!.RollbackAsync();
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    return result;
                }

                if (_depth == 1)
                {
                    await this.SaveChangesAsync();
                    await _transaction!.CommitAsync();
                    _transaction.Dispose();
                    _transaction = null;
                    _logger.LogInformation("Transaction completed successfully");
                }

                return result;
            }
            catch (Exception ex)
            {
                if (_depth == 1 && _transaction != null)
                {
                    await _transaction.RollbackAsync();
                    _transaction.Dispose();
                    _transaction = null;
                }

                _logger.LogError(ex, "Database Transaction Failed");
                return Error.Failure("Database.TransactionFailed", ex.Message);
            }
            finally
            {
                _depth--;
            }
        }
    }
}
