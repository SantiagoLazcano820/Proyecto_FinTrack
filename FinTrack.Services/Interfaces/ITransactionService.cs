using FinTrack.Core.Entities;
using FinTrack.Core.QueryFilters;

namespace FinTrack.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync(TransactionQueryFilter filters);
        Task<Transaction> GetTransactionByIdAsync(int id);
        Task InsertTransaction(Transaction transaction);
        void UpdateTransaction(Transaction transaction);
        Task<bool> DeleteTransaction(int id);
    }
}