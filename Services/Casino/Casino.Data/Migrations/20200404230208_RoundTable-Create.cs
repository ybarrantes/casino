﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Casino.Data.Migrations
{
    public partial class RoundTableCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rounds",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouletteId = table.Column<long>(nullable: false),
                    StateId = table.Column<long>(nullable: false),
                    WinNumber = table.Column<string>(maxLength: 2, nullable: true),
                    UserOpenId = table.Column<long>(nullable: false),
                    UserCloseId = table.Column<long>(nullable: true),
                    ClosedAt = table.Column<DateTime>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rounds_Roulettes_RouletteId",
                        column: x => x.RouletteId,
                        principalTable: "Roulettes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rounds_RoundStates_StateId",
                        column: x => x.StateId,
                        principalTable: "RoundStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rounds_Users_UserCloseId",
                        column: x => x.UserCloseId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rounds_Users_UserOpenId",
                        column: x => x.UserOpenId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_RouletteId",
                table: "Rounds",
                column: "RouletteId");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_StateId",
                table: "Rounds",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_UserCloseId",
                table: "Rounds",
                column: "UserCloseId");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_UserOpenId",
                table: "Rounds",
                column: "UserOpenId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rounds");
        }
    }
}
