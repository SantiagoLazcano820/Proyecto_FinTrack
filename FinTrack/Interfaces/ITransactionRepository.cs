using FinTrack.Core.DTOs;
using FinTrack.Core.Entities;

namespace FinTrack.Core.Interfaces
{
    public interface ITransactionRepository : IBaseRepository<Transaction>
    {
        Task<IEnumerable<Transaction>> GetTransactionsByUserIdDapperAsync(int userId);
        Task<IEnumerable<Transaction>> GetAllTransactionsDapperAsync();
        Task<Transaction> GetTransactionByIdDapperAsync(int id);
        Task<decimal> GetTotalBalanceByUserId(int userId);
        Task<MonthlyBalanceDto> GetMonthlyBalanceDapperAsync(int userId, int month, int year);
    }
}
