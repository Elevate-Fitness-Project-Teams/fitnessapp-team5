using AuthenticationService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthenticationService.Persistence.Configuration
{
    public sealed class LoginAttemptConfiguration
        : IEntityTypeConfiguration<LoginAttempt>
    {
        public void Configure(EntityTypeBuilder<LoginAttempt> builder)
        {
            builder.ToTable("LoginAttempts");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.IsSuccessful)
                .IsRequired();

            builder.Property(x => x.AttemptedAt)
                .IsRequired();

            builder.Property(x => x.IpAddress)
                .HasMaxLength(100);

            builder.Property(x => x.UserAgent)
                .HasMaxLength(500);

            builder.HasIndex(x => x.Email);

            builder.HasIndex(x => x.AttemptedAt);

            builder.HasOne(x => x.User)
                .WithMany(x => x.LoginAttempts)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
