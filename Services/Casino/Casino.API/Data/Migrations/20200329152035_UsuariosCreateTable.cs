using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Casino.API.Data.Migrations
{
    public partial class UsuariosCreateTable : Migration
    {
        private string trgUsuariosAfterUpdateName = "trgUsuariosAfterUpdate";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(maxLength: 128, nullable: false),
                    Email = table.Column<string>(nullable: false),
                    CloudIdentityId = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            //migrationBuilder.CreateIndex(name: "IX_Cloud_Identity_Id", table: "Usuarios", columns: new[] { "CloudIdentityId" }, unique: false);

            migrationBuilder.Sql($"CREATE TRIGGER {trgUsuariosAfterUpdateName} ON Usuarios AFTER INSERT, UPDATE AS UPDATE f set UpdatedAt = GETDATE() FROM Usuarios AS f INNER JOIN inserted AS i ON f.id = i.id;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DROP TRIGGER {trgUsuariosAfterUpdateName};");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
