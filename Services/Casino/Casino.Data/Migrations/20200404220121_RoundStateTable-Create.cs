using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Casino.Data.Migrations
{
    public partial class RoundStateTableCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoundStates",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<string>(maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoundStates", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "RouletteTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Type",
                value: "European");

            migrationBuilder.UpdateData(
                table: "RouletteTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Type",
                value: "American");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoundStates");

            migrationBuilder.UpdateData(
                table: "RouletteTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Type",
                value: "American");

            migrationBuilder.UpdateData(
                table: "RouletteTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Type",
                value: "European");
        }
    }
}
