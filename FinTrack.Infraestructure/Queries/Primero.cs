namespace FinTrack.Infrastructure.Queries
{
    public static class TransactionQueries
    {
        public static string GetRecentTransactionsMySql = @"
            SELECT Id, UserId, CategoryId, Amount, Type, Date, Description
            FROM Transaction
            WHERE UserId = @UserId
            ORDER BY Date DESC
            LIMIT @Limit;";

        public static string GetTotalByGroupMySql = @"
            SELECT Type, SUM(Amount) as Total
            FROM Transaction
            WHERE UserId = @UserId
            GROUP BY Type;";
    }
}
