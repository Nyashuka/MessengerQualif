using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DatabaseService.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOwnerUserIdOnOwnerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupChatInfos_Users_OwnerUserId",
                table: "GroupChatInfos");

            migrationBuilder.DropIndex(
                name: "IX_GroupChatInfos_ChatId",
                table: "GroupChatInfos");

            migrationBuilder.RenameColumn(
                name: "OwnerUserId",
                table: "GroupChatInfos",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupChatInfos_OwnerUserId",
                table: "GroupChatInfos",
                newName: "IX_GroupChatInfos_OwnerId");

            migrationBuilder.CreateTable(
                name: "UserDto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    ChatId = table.Column<int>(type: "integer", nullable: true)
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

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChatInfos_Users_OwnerId",
                table: "GroupChatInfos",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupChatInfos_Users_OwnerId",
                table: "GroupChatInfos");

            migrationBuilder.DropTable(
                name: "UserDto");

            migrationBuilder.DropIndex(
                name: "IX_GroupChatInfos_ChatId",
                table: "GroupChatInfos");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "GroupChatInfos",
                newName: "OwnerUserId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupChatInfos_OwnerId",
                table: "GroupChatInfos",
                newName: "IX_GroupChatInfos_OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupChatInfos_ChatId",
                table: "GroupChatInfos",
                column: "ChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChatInfos_Users_OwnerUserId",
                table: "GroupChatInfos",
                column: "OwnerUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
