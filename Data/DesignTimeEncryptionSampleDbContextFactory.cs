using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EncryptionMigrationBug.Data
{
    public sealed class DesignTimeEncryptionSampleDbContextFactory : IDesignTimeDbContextFactory<EncryptionSampleDbContext>
    {
        public EncryptionSampleDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EncryptionSampleDbContext>();
            optionsBuilder.UseSqlServer("server=.;database=EFCoreEncryptionDesignTime;Integrated Security=true;Column Encryption Setting=enabled");

            return new EncryptionSampleDbContext(optionsBuilder.Options);
        }
    }
}
