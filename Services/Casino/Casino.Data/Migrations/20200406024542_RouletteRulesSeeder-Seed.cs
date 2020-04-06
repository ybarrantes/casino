using Microsoft.EntityFrameworkCore.Migrations;

namespace Casino.Data.Migrations
{
    public partial class RouletteRulesSeederSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RouletteRuleTypes",
                columns: new[] { "Id", "DefaultPay", "Description", "Name", "Numbers", "TypeId" },
                values: new object[,]
                {
                    { 1L, 1f, null, "Red", (byte)18, 1L },
                    { 2L, 1f, null, "Black", (byte)18, 1L },
                    { 3L, 1f, null, "Odd", (byte)18, 1L },
                    { 4L, 1f, null, "Even", (byte)18, 1L },
                    { 5L, 1f, null, "1 to 18", (byte)18, 1L },
                    { 6L, 1f, null, "19 to 36", (byte)18, 1L },
                    { 7L, 2f, null, "1 to 12", (byte)12, 1L },
                    { 8L, 2f, null, "13 to 24", (byte)12, 1L },
                    { 9L, 2f, null, "25 to 36", (byte)12, 1L },
                    { 10L, 5f, null, "Six line", (byte)6, 1L },
                    { 11L, 6f, null, "First five", (byte)5, 1L },
                    { 12L, 8f, null, "Corner", (byte)4, 1L },
                    { 13L, 11f, null, "Street", (byte)3, 1L },
                    { 14L, 17f, null, "Split", (byte)2, 1L },
                    { 15L, 35f, null, "One", (byte)1, 1L }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RouletteRuleTypes",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "RouletteRuleTypes",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "RouletteRuleTypes",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "RouletteRuleTypes",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "RouletteRuleTypes",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "RouletteRuleTypes",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "RouletteRuleTypes",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "RouletteRuleTypes",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "RouletteRuleTypes",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "RouletteRuleTypes",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "RouletteRuleTypes",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "RouletteRuleTypes",
                keyColumn: "Id",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                table: "RouletteRuleTypes",
                keyColumn: "Id",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                table: "RouletteRuleTypes",
                keyColumn: "Id",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                table: "RouletteRuleTypes",
                keyColumn: "Id",
                keyValue: 15L);
        }
    }
}
