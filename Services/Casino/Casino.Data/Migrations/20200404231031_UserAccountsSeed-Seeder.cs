using Microsoft.EntityFrameworkCore.Migrations;

namespace Casino.Data.Migrations
{
    public partial class UserAccountsSeedSeeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserAccountStates",
                columns: new[] { "Id", "State" },
                values: new object[,]
                {
                    { 1L, "Active" },
                    { 2L, "Inactive" },
                    { 3L, "Suspended" }
                });

            migrationBuilder.InsertData(
                table: "UserAccountTypes",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1L, "Free" },
                    { 2L, "Real" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserAccountStates",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "UserAccountStates",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "UserAccountStates",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "UserAccountTypes",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "UserAccountTypes",
                keyColumn: "Id",
                keyValue: 2L);
        }
    }
}
