using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DatabaseService.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultRolesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DefaultRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChatId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DefaultRoles_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DefaultRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ChatPermissions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 13, "Create roles" },
                    { 14, "Give roles" },
                    { 15, "Edit roles" },
                    { 16, "Delete roles" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DefaultRoles_ChatId",
                table: "DefaultRoles",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_DefaultRoles_RoleId",
                table: "DefaultRoles",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DefaultRoles");

            migrationBuilder.DeleteData(
                table: "ChatPermissions",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ChatPermissions",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ChatPermissions",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ChatPermissions",
                keyColumn: "Id",
                keyValue: 16);
        }
    }
}
