using FCEService.Domain.Entities.UserFitnessStats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCEService.Infrastructure.Persistence.Configurations
{
    public class UserFitnessStatsConfiguration : IEntityTypeConfiguration<UserFitnessStats>
    {
        public void Configure(EntityTypeBuilder<UserFitnessStats> builder)
        {
            builder.ToTable("UserFitnessStats");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();
            
            // Unique index to ensure one active stats profile per user
            builder.HasIndex(x => x.UserId)
                .IsUnique();

            builder.Property(x => x.Weight)
                .IsRequired();

            builder.Property(x => x.Height)
                .IsRequired();

            builder.Property(x => x.BirthDate)
                .IsRequired();

            builder.Property(x => x.Gender)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.Goal)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.ActivityLevel)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Ignore(x => x.Age);

            builder.Property(x => x.CreatedBy)
                .HasMaxLength(100);

            builder.Property(x => x.LastModifiedBy)
                .HasMaxLength(100);
        }
    }
}
