using FCEService.Domain.Entities.UserAssignedPlan;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCEService.Infrastructure.Persistence.Configurations
{
    public sealed class UserAssignedPlanConfiguration : IEntityTypeConfiguration<UserAssignedPlan>
    {
        public void Configure(EntityTypeBuilder<UserAssignedPlan> builder)
        {
            builder.ToTable("UserAssignedPlans");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.PlanId)
                .IsRequired();

            builder.HasIndex(x => x.UserId);

            // Index on FK to avoid full table scan on plan-based queries
            builder.HasIndex(x => x.PlanId);

            // FK to FitnessPlanConfigs — Restrict delete to prevent orphaned assignments
            builder.HasOne<Domain.Entities.FitnessPlanConfig.FitnessPlanConfig>()
                .WithMany()
                .HasForeignKey(x => x.PlanId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.CreatedBy)
                .HasMaxLength(100);

            builder.Property(x => x.LastModifiedBy)
                .HasMaxLength(100);
        }
    }
}
