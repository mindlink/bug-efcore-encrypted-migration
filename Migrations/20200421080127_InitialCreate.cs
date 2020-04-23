using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EncryptionMigrationBug.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<string>(maxLength: 36, nullable: false),
                    GroupId = table.Column<string>(maxLength: 36, nullable: true),
                    SenderId = table.Column<string>(maxLength: 36, nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                });

            migrationBuilder.InsertData(
                table: "Messages",
                columns: new[] {"MessageId", "GroupId", "SenderId", "Content", "Timestamp"},
                values: new object[]
                {
                    Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString(),
                    "jon",
                    "This is a seeded message",
                    DateTimeOffset.UtcNow
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");
        }
    }
}
