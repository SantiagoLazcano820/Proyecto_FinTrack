using FinTrack.Core.Entities;

namespace FinTrack.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ITransactionRepository TransactionRepository { get; }
        IUserRepository UserRepository { get; }
        ICategoryRepository CategoryRepository { get; }

        void SaveChanges();
        Task SaveChangesAsync();
    }
}
