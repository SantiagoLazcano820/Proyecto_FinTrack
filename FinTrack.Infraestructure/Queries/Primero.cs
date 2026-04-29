namespace FinTrack.Infrastructure.Queries
{
    public static class Primero
    {
        // User
        public static string UserByEmailMySql = @"
        SELECT u.*, r.Id, r.Name
        FROM User u
        INNER JOIN Role r ON u.RoleId = r.Id
        WHERE u.Email = @Email;";

        public static string AllUsersMySql = "SELECT * FROM User WHERE IsActive = b'1';";
        public static string UserByIdMySql = "SELECT * FROM User WHERE Id = @Id;";

        // Category
        public static string CategoriesByUserIdMySql = @"
        SELECT Id, Name, Description, UserId, IsActive 
        FROM Category 
        WHERE UserId = @UserId AND IsActive = b'1';";

        public static string AllCategoriesMySql = "SELECT * FROM Category WHERE IsActive = b'1';";
        public static string CategoryByIdMySql = "SELECT * FROM Category WHERE Id = @Id;";

        // Transaction
        public static string TransactionsByUserIdMySql = @"
        SELECT t.*, c.Id, c.Name, c.Description
        FROM Transaction t
        INNER JOIN Category c ON t.CategoryId = c.Id
        WHERE t.UserId = @UserId
        ORDER BY t.Date DESC;";

        public static string AllTransactionsMySql = "SELECT * FROM Transaction;";
        public static string TransactionByIdMySql = "SELECT * FROM Transaction WHERE Id = @Id;";

        public static string TotalesMySql = @"
        SELECT 
            IFNULL(SUM(CASE WHEN Type = 'Income' THEN Amount ELSE 0 END), 0) AS TotalIncomes,
            IFNULL(SUM(CASE WHEN Type = 'Expense' THEN Amount ELSE 0 END), 0) AS TotalExpenses
        FROM Transaction 
        WHERE UserId = @UserId AND MONTH(Date) = @Month AND YEAR(Date) = @Year;";

        public static string DetalleMySql = @"
        SELECT c.Name AS CategoryName, SUM(t.Amount) AS Total, t.Type
        FROM Transaction t
        INNER JOIN Category c ON t.CategoryId = c.Id
        WHERE t.UserId = @UserId AND MONTH(t.Date) = @Month AND YEAR(t.Date) = @Year
        GROUP BY c.Name, t.Type
        ORDER BY Total DESC;";

        public static string GetTotalBalance = @"
        SELECT IFNULL(SUM(CASE WHEN Type = 'Income' THEN Amount ELSE -Amount END), 0) 
        FROM Transaction 
        WHERE UserId = @UserId;";
    }
}
