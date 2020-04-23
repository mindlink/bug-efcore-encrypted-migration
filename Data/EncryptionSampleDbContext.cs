using Microsoft.EntityFrameworkCore;

namespace EncryptionMigrationBug.Data
{
    public sealed class EncryptionSampleDbContext : DbContext
    {
        public EncryptionSampleDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Message> Messages => this.Set<Message>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MessageEntityTypeConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
