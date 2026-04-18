using FinTrack.Core.Entities;

namespace FinTrack.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        Task<Transaction> GetTransactionByIdAsync(int id);
        Task InsertTransaction(Transaction transaction);
        Task UpdateTransaction(Transaction transaction);
        Task<bool> DeleteTransaction(int id);
    }
}