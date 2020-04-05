using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Casino.Data.Migrations
{
    public partial class UserAccountsTablesCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAccountStates",
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
                    table.PrimaryKey("PK_UserAccountStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccountTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccountTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserOwnerId = table.Column<long>(nullable: false),
                    StateId = table.Column<long>(nullable: false),
                    TypeId = table.Column<long>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAccounts_UserAccountStates_StateId",
                        column: x => x.StateId,
                        principalTable: "UserAccountStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAccounts_UserAccountTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "UserAccountTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAccounts_Users_UserOwnerId",
                        column: x => x.UserOwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_StateId",
                table: "UserAccounts",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_TypeId",
                table: "UserAccounts",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_UserOwnerId",
                table: "UserAccounts",
                column: "UserOwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropTable(
                name: "UserAccountStates");

            migrationBuilder.DropTable(
                name: "UserAccountTypes");
        }
    }
}
