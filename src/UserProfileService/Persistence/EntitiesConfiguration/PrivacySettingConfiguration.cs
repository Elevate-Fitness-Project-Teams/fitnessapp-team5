using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserProfileService.Entities;

namespace UserProfileService.Persistence.EntitiesConfiguration;

public class PrivacySettingConfiguration : IEntityTypeConfiguration<PrivacySetting>
{
    public void Configure(EntityTypeBuilder<PrivacySetting> builder)
    {
        builder.HasKey(x => x.UserId);

        builder.Property(x => x.ProfileVisibility)
            .HasMaxLength(20);
    }
}
