using FinTrack.Core.Entities;

namespace FinTrack.Core.Interfaces
{
    public interface ITransactionRepository : IBaseRepository<Transaction>
    {
        Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(int userId);
    }
}
