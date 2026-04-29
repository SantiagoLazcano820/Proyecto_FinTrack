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
        private readonly IDapperContext _dapper;

        public UnitOfWork(FinTrackContext context, IDapperContext dapper)
        {
            _context = context;
            _dapper = dapper;
        }

        public ITransactionRepository TransactionRepository =>
            _transactionRepository ?? new TransactionRepository(_context, _dapper);

        public IUserRepository UserRepository =>
            _userRepository ?? new UserRepository(_context, _dapper);

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
