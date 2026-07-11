using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartCoachService.Entities;

namespace SmartCoachService.Persistence.Configurations
{
    public sealed class ChatSessionConfiguration : IEntityTypeConfiguration<ChatSession>
    {
        public void Configure(EntityTypeBuilder<ChatSession> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                   .HasMaxLength(200);

            builder.HasMany(x => x.Messages)
                   .WithOne(x => x.ChatSession)
                   .HasForeignKey(x => x.ChatSessionId);
        }
    }
}
