using Microsoft.EntityFrameworkCore.Migrations;

namespace Casino.Data.Migrations
{
    public partial class RoulettesSeedSeeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RouletteStates",
                columns: new[] { "Id", "State" },
                values: new object[,]
                {
                    { 1L, "Active" },
                    { 2L, "Inactive" },
                    { 3L, "Suspended" }
                });

            migrationBuilder.InsertData(
                table: "RouletteTypes",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1L, "American" },
                    { 2L, "European" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RouletteStates",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "RouletteStates",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "RouletteStates",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "RouletteTypes",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "RouletteTypes",
                keyColumn: "Id",
                keyValue: 2L);
        }
    }
}
