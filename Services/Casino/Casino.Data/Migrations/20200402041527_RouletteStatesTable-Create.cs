using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Casino.Data.Migrations
{
    public partial class RouletteStatesTableCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RouletteStates",
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
                    table.PrimaryKey("PK_RouletteStates", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RouletteStates");
        }
    }
}
