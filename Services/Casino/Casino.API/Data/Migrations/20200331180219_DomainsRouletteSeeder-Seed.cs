using Casino.API.Data.Seeders;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Casino.API.Data.Migrations
{
    public partial class DomainsRouletteSeederSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            AddRoulettesDomainsSeed.AddDomains(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
