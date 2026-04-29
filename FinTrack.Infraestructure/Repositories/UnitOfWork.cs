using FinTrack.Core.Interfaces;
using FinTrack.Infraestructure.Data;
using FinTrack.Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace FinTrack.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FinTrackContext _context;
        private readonly IDapperContext _dapper;
        private ITransactionRepository _transactionRepository;
        private IUserRepository _userRepository;
        private ICategoryRepository _categoryRepository;

        private IDbContextTransaction _efTransaction;

        public UnitOfWork(FinTrackContext context, IDapperContext dapper)
        {
            _context = context;
            _dapper = dapper;
        }

        public ITransactionRepository TransactionRepository =>
            _transactionRepository ??= new TransactionRepository(_context, _dapper);

        public IUserRepository UserRepository =>
            _userRepository ??= new UserRepository(_context, _dapper);

        public ICategoryRepository CategoryRepository =>
            _categoryRepository ??= new CategoryRepository(_context, _dapper);

        public void Dispose()
        {
            _efTransaction?.Dispose();
            _context?.Dispose();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        #region Transacciones
        public async Task BeginTransactionAsync()
        {
            if (_efTransaction == null)
            {
                _efTransaction = await _context.Database.BeginTransactionAsync();
                var conn = _context.Database.GetDbConnection();
                var tx = _efTransaction.GetDbTransaction();
                _dapper.SetAmbientConnection(conn, tx);
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_efTransaction != null)
                {
                    await _efTransaction.CommitAsync();
                    _efTransaction.Dispose();
                    _efTransaction = null;
                }
            }
            finally
            {
                _dapper.ClearAmbientConnection();
            }
        }

        public async Task RollbackAsync()
        {
            if (_efTransaction != null)
            {
                await _efTransaction.RollbackAsync();
                _efTransaction.Dispose();
                _efTransaction = null;
            }
            _dapper.ClearAmbientConnection();
        }

        public IDbConnection? GetDbConnection()
        {
            return _context.Database.GetDbConnection();
        }

        public IDbTransaction? GetDbTransaction()
        {
            return _efTransaction?.GetDbTransaction();
        }
        #endregion
    }
}

