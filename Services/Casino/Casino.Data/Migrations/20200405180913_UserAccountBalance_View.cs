using Microsoft.EntityFrameworkCore.Migrations;

namespace Casino.Data.Migrations
{
    public partial class UserAccountBalance_View : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW UserAccountsBalance(
                    Id,
                    TotalBalance,
                    BetBalance,
                    AvailableBalance
                )
                AS
                SELECT
                    a.Id,
                    SUM(COALESCE(t.Amount, 0)) AS TotalBalance,
                    SUM(
                        CASE
                            WHEN t.TypeId = 3 AND t.StateId = 2 THEN COALESCE(t.Amount, 0)
                            ELSE 0
                        END
                    ) AS BetBalance,
                    SUM(
                        CASE
                            WHEN t.TypeId = 3 AND t.StateId = 2 THEN 0
                            ELSE COALESCE(t.Amount, 0)
                        END
                    ) AS AvailableBalance
                FROM UserAccounts AS a
                LEFT JOIN AccountTransactions AS t
                    ON a.Id = t.UserAccountId
                    -- all transactions with approved state or 
                    -- transactions in pending state and bet type
                    AND (t.StateId = 1 OR (t.StateId = 2 AND t.TypeId = 3))
                    AND t.DeletedAt IS NULL
                WHERE
                    a.DeletedAt IS NULL
                GROUP BY
                    a.Id;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS UserAccountsBalance");
        }
    }
}
