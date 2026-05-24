using FinTrack.Core.CustomEntities;
using FinTrack.Core.DTOs;
using FinTrack.Core.Entities;
using FinTrack.Core.QueryFilters;

namespace FinTrack.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<ResponseData> GetAllTransactionsAsync(TransactionQueryFilter filters);
        Task<Transaction> GetTransactionByIdAsync(int id);
        Task<ResponseData> GetAllTransactionsDapperAsync(TransactionQueryFilter filters);
        Task InsertTransaction(Transaction transaction);
        void UpdateTransaction(Transaction transaction);
        Task<bool> DeleteTransaction(int id);
        Task<MonthlyBalanceDto> GetMonthlyBalance(int userId, int month, int year);
    }
}