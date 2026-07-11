using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationService.Entities;

namespace NotificationService.Persistence.EntitiesConfiguration;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.Property(x => x.Title)
            .HasMaxLength(150);

        builder.Property(x => x.Message)
            .HasMaxLength(1000);

        builder.Property(x => x.Type)
            .HasMaxLength(50);

        builder.HasIndex(x => x.UserId);
    }
}
