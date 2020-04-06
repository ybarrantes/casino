using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Casino.Data.Migrations
{
    public partial class BetsTables_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BetStates",
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
                    table.PrimaryKey("PK_BetStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bets",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoundId = table.Column<long>(nullable: false),
                    AccountTransactionId = table.Column<long>(nullable: false),
                    RouletteRuleId = table.Column<long>(nullable: false),
                    StateId = table.Column<long>(nullable: false),
                    UserRegisterId = table.Column<long>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bets_AccountTransactions_AccountTransactionId",
                        column: x => x.AccountTransactionId,
                        principalTable: "AccountTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bets_RouletteRules_RouletteRuleId",
                        column: x => x.RouletteRuleId,
                        principalTable: "RouletteRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bets_Rounds_RoundId",
                        column: x => x.RoundId,
                        principalTable: "Rounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bets_BetStates_StateId",
                        column: x => x.StateId,
                        principalTable: "BetStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bets_Users_UserRegisterId",
                        column: x => x.UserRegisterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BetNumbers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BetId = table.Column<long>(nullable: false),
                    NumberId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BetNumbers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BetNumbers_Bets_BetId",
                        column: x => x.BetId,
                        principalTable: "Bets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BetNumbers_Numbers_NumberId",
                        column: x => x.NumberId,
                        principalTable: "Numbers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BetNumbers_BetId",
                table: "BetNumbers",
                column: "BetId");

            migrationBuilder.CreateIndex(
                name: "IX_BetNumbers_NumberId",
                table: "BetNumbers",
                column: "NumberId");

            migrationBuilder.CreateIndex(
                name: "IX_Bets_AccountTransactionId",
                table: "Bets",
                column: "AccountTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Bets_RouletteRuleId",
                table: "Bets",
                column: "RouletteRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Bets_RoundId",
                table: "Bets",
                column: "RoundId");

            migrationBuilder.CreateIndex(
                name: "IX_Bets_StateId",
                table: "Bets",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Bets_UserRegisterId",
                table: "Bets",
                column: "UserRegisterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BetNumbers");

            migrationBuilder.DropTable(
                name: "Bets");

            migrationBuilder.DropTable(
                name: "BetStates");
        }
    }
}
