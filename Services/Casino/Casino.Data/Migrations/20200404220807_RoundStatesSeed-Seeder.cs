using Microsoft.EntityFrameworkCore.Migrations;

namespace Casino.Data.Migrations
{
    public partial class RoundStatesSeedSeeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RoundStates",
                columns: new[] { "Id", "State" },
                values: new object[] { 1L, "Opened" });

            migrationBuilder.InsertData(
                table: "RoundStates",
                columns: new[] { "Id", "State" },
                values: new object[] { 2L, "Closed" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoundStates",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "RoundStates",
                keyColumn: "Id",
                keyValue: 2L);
        }
    }
}
