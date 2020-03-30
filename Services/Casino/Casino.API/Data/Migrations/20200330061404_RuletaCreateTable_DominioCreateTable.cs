using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Casino.API.Data.Migrations
{
    public partial class RuletaCreateTable_DominioCreateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Usuarios",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Usuarios",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "Dominios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(maxLength: 200, nullable: true),
                    PadreId = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dominios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dominios_Dominios_PadreId",
                        column: x => x.PadreId,
                        principalTable: "Dominios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ruletas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(maxLength: 200, nullable: true),
                    UsuarioRegistraIdId = table.Column<long>(nullable: false),
                    EstadoId = table.Column<int>(nullable: false),
                    TipoId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ruletas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ruletas_Dominios_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Dominios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ruletas_Dominios_TipoId",
                        column: x => x.TipoId,
                        principalTable: "Dominios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ruletas_Usuarios_UsuarioRegistraIdId",
                        column: x => x.UsuarioRegistraIdId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dominios_PadreId",
                table: "Dominios",
                column: "PadreId");

            migrationBuilder.CreateIndex(
                name: "IX_Ruletas_EstadoId",
                table: "Ruletas",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Ruletas_TipoId",
                table: "Ruletas",
                column: "TipoId");

            migrationBuilder.CreateIndex(
                name: "IX_Ruletas_UsuarioRegistraIdId",
                table: "Ruletas",
                column: "UsuarioRegistraIdId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ruletas");

            migrationBuilder.DropTable(
                name: "Dominios");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Usuarios",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Usuarios",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
