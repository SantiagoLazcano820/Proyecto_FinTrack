using FinTrack.Core.Entities;
using FinTrack.Core.Interfaces;
using FinTrack.Infraestructure.Data;
using FinTrack.Infraestructure.Repositories;

namespace FinTrack.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FinTrackContext _context;
        private ITransactionRepository _transactionRepository;
        private IUserRepository _userRepository;
        private ICategoryRepository _categoryRepository;

        public UnitOfWork(FinTrackContext context)
        {
            _context = context;
        }

        public ITransactionRepository TransactionRepository =>
            _transactionRepository ?? new TransactionRepository(_context);

        public IUserRepository UserRepository =>
            _userRepository ?? new UserRepository(_context);

        public ICategoryRepository CategoryRepository =>
            _categoryRepository ?? new CategoryRepository(_context);

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
