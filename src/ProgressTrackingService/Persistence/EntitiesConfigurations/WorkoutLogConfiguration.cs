using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgressTrackingService.Entities;

namespace ProgressTrackingService.Persistence.EntitiesConfigurations;

public class WorkoutLogConfiguration : IEntityTypeConfiguration<WorkoutLog>
{
    public void Configure(EntityTypeBuilder<WorkoutLog> builder)
    {
        builder.Property(wl => wl.SessionId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(wl => wl.Notes)
            .HasMaxLength(500);

        builder.Property(x => x.CompletedAt)
            .HasColumnType("datetime2");

        builder.HasIndex(x => x.UserId);

        builder.HasIndex(wl => new { wl.UserId, wl.CompletedAt });
    }
}
