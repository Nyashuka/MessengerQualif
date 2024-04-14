using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MessengerDatabaseService.Migrations
{
    /// <inheritdoc />
    public partial class FixReferenceKeysinAllTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMembers_ChatTypes_ChatId",
                table: "ChatMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ChatTypes_ChatId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_ChatTypes_ChatId",
                table: "Roles");

            migrationBuilder.CreateTable(
                name: "Friends",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<int>(type: "integer", nullable: false),
                    FriendAccountId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friends", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Friends_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Friends_Accounts_FriendAccountId",
                        column: x => x.FriendAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessTokens_AccountId",
                table: "AccessTokens",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_AccountId",
                table: "Friends",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_FriendAccountId",
                table: "Friends",
                column: "FriendAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessTokens_Accounts_AccountId",
                table: "AccessTokens",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMembers_Chats_ChatId",
                table: "ChatMembers",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Chats_ChatId",
                table: "Roles",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessTokens_Accounts_AccountId",
                table: "AccessTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMembers_Chats_ChatId",
                table: "ChatMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Chats_ChatId",
                table: "Roles");

            migrationBuilder.DropTable(
                name: "Friends");

            migrationBuilder.DropIndex(
                name: "IX_AccessTokens_AccountId",
                table: "AccessTokens");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMembers_ChatTypes_ChatId",
                table: "ChatMembers",
                column: "ChatId",
                principalTable: "ChatTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ChatTypes_ChatId",
                table: "Messages",
                column: "ChatId",
                principalTable: "ChatTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_ChatTypes_ChatId",
                table: "Roles",
                column: "ChatId",
                principalTable: "ChatTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
