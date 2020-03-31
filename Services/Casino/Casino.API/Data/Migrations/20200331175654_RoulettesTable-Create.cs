using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Casino.API.Data.Migrations
{
    public partial class RoulettesTableCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roulettes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    UserRegisterId = table.Column<long>(nullable: false),
                    StateId = table.Column<int>(nullable: false),
                    TypeId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roulettes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roulettes_Domains_StateId",
                        column: x => x.StateId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Roulettes_Domains_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Roulettes_Users_UserRegisterId",
                        column: x => x.UserRegisterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Roulettes_StateId",
                table: "Roulettes",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Roulettes_TypeId",
                table: "Roulettes",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Roulettes_UserRegisterId",
                table: "Roulettes",
                column: "UserRegisterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Roulettes");
        }
    }
}
