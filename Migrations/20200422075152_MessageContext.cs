using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EncryptionMigrationBug.Migrations
{
    public partial class MessageContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Context",
                table: "Messages",
                nullable: true);

            migrationBuilder.InsertData("Messages",
                new[] {"MessageId", "GroupId", "SenderId", "Content", "Timestamp", "Context"},
                new object[]
                {
                    Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
                    "This is content inserted as part of a migration", DateTimeOffset.UtcNow, "Some context"
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Context",
                table: "Messages");
        }
    }
}
