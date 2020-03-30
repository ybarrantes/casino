using Casino.API.Data.Queries;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Casino.API.Data.Migrations
{
    public partial class DominiosRuletaSeeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            InsertDefaultDomainsQuery.addDomains(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
