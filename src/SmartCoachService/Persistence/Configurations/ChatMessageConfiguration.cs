using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartCoachService.Entities;

namespace SmartCoachService.Persistence.Configurations
{
    public sealed class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Message)
                   .HasMaxLength(2000)
                   .IsRequired();

            builder.Property(x => x.Sender)
                   .HasConversion<string>();
        }
    }
}
