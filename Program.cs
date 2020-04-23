using System;
using System.Linq;
using System.Threading.Tasks;
using EncryptionMigrationBug.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace EncryptionMigrationBug
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var databaseConnectionString = "Data Source=.;Initial Catalog=EFCoreEncryption;Integrated Security=True;Column Encryption Setting=enabled";

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<EncryptionSampleDbContext>(options =>
                options.UseSqlServer(databaseConnectionString));

            await using var provider = serviceCollection.BuildServiceProvider();

            var steps = new (string command, string prompt, Func<Task> fn)[]
            {
                ("create", "The first step is to create the database, say '{0}' to create it: ",
                    () => CreateDatabaseAsync(provider)),
                ("encrypted",
                    "Now we need to encrypt the database, go off and run 'configuredb.ps1', then say '{0}' to proceed: ",
                    () => Task.CompletedTask),
                ("migrate", "Next let's migrate the database, say '{0}' to migrate it: ",
                    () => MigrateDatabaseAsync(provider)),
                ("test",
                    "Finally we can test the database, check the content is encrypted then say '{0}' to see the unencrypted content: ",
                    () => TestDatabaseAsync(provider)),
                ("exit", "You're done! Say '{0}' and you can go! ", () => Task.CompletedTask)
            };

            var step = 0;
            while (step < steps.Length)
            {
                var (command, prompt, fn) = steps[step];
                Console.Write(prompt, command);

                var input = Console.ReadLine();

                if (string.Equals(input, command, StringComparison.OrdinalIgnoreCase))
                {
                    await fn();
                    ++step;
                }
            }
        }

        private static async Task CreateDatabaseAsync(IServiceProvider provider)
        {
            ApplyConsoleColor(() => Console.WriteLine("Creating database at initial migration..."), ConsoleColor.Gray);

            using (var scope = provider.CreateScope())
            {
                await using var context = scope.ServiceProvider.GetService<EncryptionSampleDbContext>();

                var availableMigrations = context.Database.GetMigrations();

                var migrator = context.Database.GetService<IMigrator>();

                await migrator.MigrateAsync(availableMigrations.First());
            }

            ApplyConsoleColor(() => Console.WriteLine("Created database at initial migration."), ConsoleColor.Green);
        }

        private static async Task TestDatabaseAsync(IServiceProvider provider)
        {
            using (var scope = provider.CreateScope())
            {
                await using var context = scope.ServiceProvider.GetService<EncryptionSampleDbContext>();
                context.Messages.Add(new Message
                {
                    MessageId = Guid.NewGuid().ToString(),
                    GroupId = Guid.NewGuid().ToString(),
                    SenderId = Guid.NewGuid().ToString(),
                    Content = "This message should have been encrypted and was added after migration.",
                    Timestamp = DateTimeOffset.UtcNow
                });

                await context.SaveChangesAsync();
            }

            using (var scope = provider.CreateScope())
            {
                await using var context = scope.ServiceProvider.GetService<EncryptionSampleDbContext>();
                var recentTime = DateTimeOffset.UtcNow - TimeSpan.FromDays(1);
                var messages = await context.Messages.Where(m => m.Timestamp >= recentTime)
                    .ToListAsync();

                Console.WriteLine("The messages are:");
                foreach (var message in messages)
                {
                    Console.WriteLine($"{message.MessageId} => (" +
                                      $"{message.GroupId}) {message.Timestamp} {message.SenderId}: {message.Content}");
                }
            }

            Console.WriteLine();
        }

        private static async Task MigrateDatabaseAsync(IServiceProvider provider)
        {
            ApplyConsoleColor(() => Console.WriteLine("Migrating database to latest migration..."), ConsoleColor.Gray);

            using (var scope = provider.CreateScope())
            {
                await using var context = scope.ServiceProvider.GetService<EncryptionSampleDbContext>();

                await context.Database.MigrateAsync();
            }

            ApplyConsoleColor(() => Console.WriteLine("Migrated database to latest migration."), ConsoleColor.Green);
        }

        private static void ApplyConsoleColor(Action consoleAction, ConsoleColor color)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            consoleAction();
            Console.ForegroundColor = previousColor;
        }
    }
}
