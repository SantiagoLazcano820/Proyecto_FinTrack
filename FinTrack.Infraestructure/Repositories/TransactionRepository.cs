using FinTrack.Core.Entities;
using FinTrack.Core.Interfaces;
using FinTrack.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infraestructure.Repositories
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(FinTrackContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(int userId)
        {
            return await _entities.Where(x => x.UserId == userId).Include(x => x.Category).ToListAsync();
        }
    }
}
