using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserProfileService.Entities;

namespace UserProfileService.Persistence.EntitiesConfiguration;

public class UserPreferenceConfiguration : IEntityTypeConfiguration<UserPreference>
{
    public void Configure(EntityTypeBuilder<UserPreference> builder)
    {
        builder.HasKey(x => x.UserId);

        builder.Property(x => x.Language)
           .HasMaxLength(10);

        builder.Property(x => x.Theme)
            .HasMaxLength(15);

        builder.Property(x => x.WeightUnit)
            .HasMaxLength(5);

        builder.Property(x => x.HeightUnit)
            .HasMaxLength(5);

        builder.Property(x => x.DistanceUnit)
            .HasMaxLength(5);
    }
}
