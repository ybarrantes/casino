using Microsoft.EntityFrameworkCore.Migrations;

namespace Casino.Data.Migrations
{
    public partial class SystemUserSeed_Seeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CloudIdentityId", "DeletedAt", "Email", "Username" },
                values: new object[] { 1L, "1ca11719-2566-4da7-87f3-fc65ddc591ad", null, "ybarrantes.juntos085@gmail.com", "system-1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L);
        }
    }
}
