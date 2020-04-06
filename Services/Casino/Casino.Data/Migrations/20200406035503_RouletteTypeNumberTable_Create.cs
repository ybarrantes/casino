using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Casino.Data.Migrations
{
    public partial class RouletteTypeNumberTable_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RouletteTypeNumbers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeId = table.Column<long>(nullable: false),
                    NumberId = table.Column<long>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouletteTypeNumbers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouletteTypeNumbers_Numbers_NumberId",
                        column: x => x.NumberId,
                        principalTable: "Numbers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RouletteTypeNumbers_RouletteTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "RouletteTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RouletteTypeNumbers_NumberId",
                table: "RouletteTypeNumbers",
                column: "NumberId");

            migrationBuilder.CreateIndex(
                name: "IX_RouletteTypeNumbers_TypeId",
                table: "RouletteTypeNumbers",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RouletteTypeNumbers");
        }
    }
}
