using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserProfileService.Entities;

namespace UserProfileService.Persistence.EntitiesConfiguration;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasKey(x => x.UserId);

        builder.Property(x => x.FirstName).HasMaxLength(50);
        builder.Property(x => x.LastName).HasMaxLength(50);
        builder.Property(x => x.Email).HasMaxLength(250);
        builder.Property(x => x.Phone).HasMaxLength(20);
        builder.Property(x => x.ProfilePictureUrl).HasMaxLength(500);

        builder.HasOne(x => x.Preference)
           .WithOne(x => x.UserProfile)
           .HasForeignKey<UserPreference>(x => x.UserId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.NotificationSetting)
           .WithOne(x => x.UserProfile)
           .HasForeignKey<NotificationSetting>(x => x.UserId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.PrivacySetting)
            .WithOne(x => x.UserProfile)
            .HasForeignKey<PrivacySetting>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
