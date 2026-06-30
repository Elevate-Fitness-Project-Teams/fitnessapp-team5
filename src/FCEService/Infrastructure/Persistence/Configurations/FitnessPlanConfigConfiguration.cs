using FCEService.Domain.Entities.FitnessPlanConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCEService.Infrastructure.Persistence.Configurations
{
    public class FitnessPlanConfigConfiguration : IEntityTypeConfiguration<FitnessPlanConfig>
    {
        public void Configure(EntityTypeBuilder<FitnessPlanConfig> builder)
        {
            builder.ToTable("FitnessPlanConfigs");

            builder.HasKey(x => x.PlanId);

            builder.Property(x => x.PlanId)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.PlanName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.Goal)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.MinCalorie)
                .IsRequired();

            builder.Property(x => x.MaxCalorie)
                .IsRequired();

            builder.Property(x => x.EstimatedDuration)
                .HasMaxLength(50);

            builder.Property(x => x.WorkoutsPerWeek)
                .IsRequired();

            builder.Property(x => x.ProgramType)
                .HasMaxLength(50);

            builder.Property(x => x.CreatedBy)
                .HasMaxLength(100);

            builder.Property(x => x.LastModifiedBy)
                .HasMaxLength(100);

            // Index for filtering plans by Goal
            builder.HasIndex(x => x.Goal);

            // Composite index for filtering by Goal + FitnessStatus together
            builder.HasIndex(x => new { x.Goal, x.Status });
        }
    }
}

