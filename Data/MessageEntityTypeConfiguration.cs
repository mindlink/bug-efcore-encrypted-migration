using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EncryptionMigrationBug.Data
{
    internal sealed class MessageEntityTypeConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(m => m.MessageId);
            builder.Property(m => m.MessageId).HasMaxLength(36);
            builder.Property(m => m.Content).IsUnicode();
            builder.Property(m => m.GroupId).HasMaxLength(36);
            builder.Property(m => m.SenderId).HasMaxLength(36);
            builder.Property(m => m.Timestamp);
        }
    }
}
