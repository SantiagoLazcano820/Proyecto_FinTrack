using FinTrack.Core.Entities;
using System.Data;

namespace FinTrack.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ITransactionRepository TransactionRepository { get; }
        IUserRepository UserRepository { get; }
        ICategoryRepository CategoryRepository { get; }

        void SaveChanges();
        Task SaveChangesAsync();

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();

        IDbConnection GetDbConnection();
        IDbTransaction? GetDbTransaction();
    }
}
