using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DatabaseService.Migrations
{
    /// <inheritdoc />
    public partial class AddPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDto");

            migrationBuilder.DropIndex(
                name: "IX_GroupChatInfos_ChatId",
                table: "GroupChatInfos");

            migrationBuilder.InsertData(
                table: "ChatPermissions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Send text messages" },
                    { 2, "Send photos" },
                    { 3, "Send video" },
                    { 10, "Add members" },
                    { 11, "Delete members" },
                    { 12, "Ban members" },
                    { 50, "Change chat info" },
                    { 100, "Administrator" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupChatInfos_ChatId",
                table: "GroupChatInfos",
                column: "ChatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GroupChatInfos_ChatId",
                table: "GroupChatInfos");

            migrationBuilder.DeleteData(
                table: "ChatPermissions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ChatPermissions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ChatPermissions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ChatPermissions",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ChatPermissions",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ChatPermissions",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ChatPermissions",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "ChatPermissions",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.CreateTable(
                name: "UserDto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChatId = table.Column<int>(type: "integer", nullable: true),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDto_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupChatInfos_ChatId",
                table: "GroupChatInfos",
                column: "ChatId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDto_ChatId",
                table: "UserDto",
                column: "ChatId");
        }
    }
}
