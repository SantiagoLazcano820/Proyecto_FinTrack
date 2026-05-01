using FinTrack.Core.DTOs;
using FinTrack.Core.Entities;
using FinTrack.Core.QueryFilters;

namespace FinTrack.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync(TransactionQueryFilter filters);
        Task<Transaction> GetTransactionByIdAsync(int id);
        Task<IEnumerable<Transaction>> GetAllTransactionsDapperAsync(TransactionQueryFilter filters);
        Task<Transaction> GetTransactionByIdDapperAsync(int id);
        Task InsertTransaction(Transaction transaction);
        void UpdateTransaction(Transaction transaction);
        Task<bool> DeleteTransaction(int id);
        Task<MonthlyBalanceDto> GetMonthlyBalance(int userId, int month, int year);
    }
}