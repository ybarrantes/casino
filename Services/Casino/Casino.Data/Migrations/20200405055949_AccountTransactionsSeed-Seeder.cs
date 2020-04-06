using Microsoft.EntityFrameworkCore.Migrations;

namespace Casino.Data.Migrations
{
    public partial class AccountTransactionsSeedSeeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AccountTransactionStates",
                columns: new[] { "Id", "State" },
                values: new object[,]
                {
                    { 1L, "Approved" },
                    { 2L, "Pending" },
                    { 3L, "Cancelled" },
                    { 4L, "Rejected" }
                });

            migrationBuilder.InsertData(
                table: "AccountTransactionTypes",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1L, "Deposit" },
                    { 2L, "Withdrawal" },
                    { 3L, "Bet" },
                    { 4L, "Bonus" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccountTransactionStates",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "AccountTransactionStates",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "AccountTransactionStates",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "AccountTransactionStates",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "AccountTransactionTypes",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "AccountTransactionTypes",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "AccountTransactionTypes",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "AccountTransactionTypes",
                keyColumn: "Id",
                keyValue: 4L);
        }
    }
}
