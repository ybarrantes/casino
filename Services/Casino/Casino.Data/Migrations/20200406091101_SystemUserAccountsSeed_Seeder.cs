using Microsoft.EntityFrameworkCore.Migrations;

namespace Casino.Data.Migrations
{
    public partial class SystemUserAccountsSeed_Seeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserAccounts",
                columns: new[] { "Id", "DeletedAt", "StateId", "TypeId", "UserOwnerId" },
                values: new object[] { 1L, null, 1L, 1L, 1L });

            migrationBuilder.InsertData(
                table: "UserAccounts",
                columns: new[] { "Id", "DeletedAt", "StateId", "TypeId", "UserOwnerId" },
                values: new object[] { 2L, null, 1L, 2L, 1L });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 2L);
        }
    }
}
