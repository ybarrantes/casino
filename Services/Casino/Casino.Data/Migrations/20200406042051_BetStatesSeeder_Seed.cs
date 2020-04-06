using Microsoft.EntityFrameworkCore.Migrations;

namespace Casino.Data.Migrations
{
    public partial class BetStatesSeeder_Seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BetStates",
                columns: new[] { "Id", "State" },
                values: new object[] { 1L, "Active" });

            migrationBuilder.InsertData(
                table: "BetStates",
                columns: new[] { "Id", "State" },
                values: new object[] { 2L, "Canceled" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BetStates",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "BetStates",
                keyColumn: "Id",
                keyValue: 2L);
        }
    }
}
