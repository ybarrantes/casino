using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Casino.Data.Migrations
{
    public partial class RouletteRulesTablesCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RouletteRuleTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    TypeId = table.Column<long>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    DefaultPay = table.Column<float>(nullable: false),
                    Numbers = table.Column<byte>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouletteRuleTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouletteRuleTypes_RouletteTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "RouletteTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RouletteRules",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouletteId = table.Column<long>(nullable: false),
                    TypeId = table.Column<long>(nullable: false),
                    Pay = table.Column<float>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouletteRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouletteRules_Roulettes_RouletteId",
                        column: x => x.RouletteId,
                        principalTable: "Roulettes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RouletteRules_RouletteRuleTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "RouletteRuleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RouletteRules_RouletteId",
                table: "RouletteRules",
                column: "RouletteId");

            migrationBuilder.CreateIndex(
                name: "IX_RouletteRules_TypeId",
                table: "RouletteRules",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RouletteRuleTypes_TypeId",
                table: "RouletteRuleTypes",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RouletteRules");

            migrationBuilder.DropTable(
                name: "RouletteRuleTypes");
        }
    }
}
