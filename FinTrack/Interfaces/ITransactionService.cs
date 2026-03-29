using FinTrack.Core.DTOs;

namespace FinTrack.Core.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> GetTransactionsAsync();
        Task<TransactionDto> RegisterTransactionAsync(TransactionDto transactionDto);
        Task<decimal> GetUserBalanceAsync(int userId);
        Task<bool> UpdateTransactionAsync(int id, TransactionDto dto);
        Task<bool> DeleteTransactionAsync(int id);
    }
}
