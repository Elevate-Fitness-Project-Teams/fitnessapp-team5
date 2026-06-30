using FCEService.Domain.Entities.CalculatedMetrics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCEService.Infrastructure.Persistence.Configurations
{
    public class CalculatedMetricsConfiguration : IEntityTypeConfiguration<CalculatedMetrics>
    {
        public void Configure(EntityTypeBuilder<CalculatedMetrics> builder)
        {
            builder.ToTable("CalculatedMetrics");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            // Unique Index to ensure only one calculated metrics record exists per user
            builder.HasIndex(x => x.UserId)
                .IsUnique();

            builder.Property(x => x.Bmr)
                .IsRequired();

            builder.Property(x => x.Tdee)
                .IsRequired();

            builder.Property(x => x.CalorieTarget)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.CreatedBy)
                .HasMaxLength(100);

            builder.Property(x => x.LastModifiedBy)
                .HasMaxLength(100);
        }
    }
}
