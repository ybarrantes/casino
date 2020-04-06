using Microsoft.EntityFrameworkCore.Migrations;

namespace Casino.Data.Migrations
{
    public partial class RoundTable_ModifyWinNumberType_AlterTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WinNumber",
                table: "Rounds");

            migrationBuilder.AddColumn<long>(
                name: "WinNumberId",
                table: "Rounds",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_WinNumberId",
                table: "Rounds",
                column: "WinNumberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Numbers_WinNumberId",
                table: "Rounds",
                column: "WinNumberId",
                principalTable: "Numbers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Numbers_WinNumberId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_WinNumberId",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "WinNumberId",
                table: "Rounds");

            migrationBuilder.AddColumn<string>(
                name: "WinNumber",
                table: "Rounds",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true);
        }
    }
}
