using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartCoachService.Entities;

namespace SmartCoachService.Persistence.Configurations
{
    public sealed class RecommendationCacheConfiguration
        : IEntityTypeConfiguration<RecommendationCache>
    {
        public void Configure(
            EntityTypeBuilder<RecommendationCache> builder)
        {
            builder.ToTable("RecommendationCaches");

            builder.HasKey(x => x.UserId);

            builder.Property(x => x.UserId)
                   .ValueGeneratedNever();

            builder.Property(x => x.UserContextJson)
                   .HasColumnType("nvarchar(max)")
                   .IsRequired();

            builder.Property(x => x.HomeFeedDataJson)
                   .HasColumnType("nvarchar(max)")
                   .IsRequired();

            builder.Property(x => x.CachedAt)
                   .IsRequired();

            builder.Property(x => x.ExpiresAt)
                   .IsRequired();
        }
    }
}