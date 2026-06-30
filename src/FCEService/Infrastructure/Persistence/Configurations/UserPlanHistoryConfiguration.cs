using FCEService.Domain.Entities.UserPlanHistory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCEService.Infrastructure.Persistence.Configurations
{
    public class UserPlanHistoryConfiguration : IEntityTypeConfiguration<UserPlanHistory>
    {
        public void Configure(EntityTypeBuilder<UserPlanHistory> builder)
        {
            builder.ToTable("UserPlanHistories");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.HasIndex(x => x.UserId);

            builder.Property(x => x.PlanId)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.ReasonForChange)
                .HasMaxLength(255)
                .IsRequired();

            // FK to FitnessPlanConfigs — Restrict delete to keep history intact
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
