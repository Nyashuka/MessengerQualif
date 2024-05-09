using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DatabaseService.Migrations
{
    /// <inheritdoc />
    public partial class CreateChatPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupChatInfos_Users_OwnerUserId",
                table: "GroupChatInfos");

            migrationBuilder.RenameColumn(
                name: "OwnerUserId",
                table: "GroupChatInfos",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupChatInfos_OwnerUserId",
                table: "GroupChatInfos",
                newName: "IX_GroupChatInfos_OwnerId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ChatPermissions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "ChatPermissions",
                type: "integer",
                nullable: true);

            migrationBuilder.InsertData(
                table: "ChatPermissions",
                columns: new[] { "Id", "Name", "RoleId" },
                values: new object[,]
                {
                    { 1, "Send text messages", null },
                    { 2, "Send photos", null },
                    { 3, "Send video", null },
                    { 10, "Add members", null },
                    { 11, "Delete members", null },
                    { 12, "Ban members", null },
                    { 50, "Change chat info", null },
                    { 100, "Administrator", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatPermissions_RoleId",
                table: "ChatPermissions",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatPermissions_Roles_RoleId",
                table: "ChatPermissions",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id");

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
                name: "FK_ChatPermissions_Roles_RoleId",
                table: "ChatPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupChatInfos_Users_OwnerId",
                table: "GroupChatInfos");

            migrationBuilder.DropIndex(
                name: "IX_ChatPermissions_RoleId",
                table: "ChatPermissions");

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

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "ChatPermissions");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "GroupChatInfos",
                newName: "OwnerUserId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupChatInfos_OwnerId",
                table: "GroupChatInfos",
                newName: "IX_GroupChatInfos_OwnerUserId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ChatPermissions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

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
