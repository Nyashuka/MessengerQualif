using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DatabaseService.Migrations
{
    /// <inheritdoc />
    public partial class clearbadpermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ChatPermissions",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ChatPermissions",
                keyColumn: "Id",
                keyValue: 15);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ChatPermissions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 14, "Give roles" },
                    { 15, "Edit roles" }
                });
        }
    }
}
