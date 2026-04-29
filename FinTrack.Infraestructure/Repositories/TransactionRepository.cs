using FinTrack.Core.DTOs;
using FinTrack.Core.Entities;
using FinTrack.Core.Enum;
using FinTrack.Core.Interfaces;
using FinTrack.Infraestructure.Data;
using FinTrack.Infrastructure.Queries;

namespace FinTrack.Infraestructure.Repositories
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        private readonly IDapperContext _dapper;

        public TransactionRepository(FinTrackContext context, IDapperContext dapper) : base(context)
        {
            _dapper = dapper;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByUserIdDapperAsync(int userId)
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DataBaseProvider.MySql => Primero.TransactionsByUserIdMySql,
                    _ => throw new NotSupportedException("Provider no soportado")
                };

                return await _dapper.QueryAsync<Transaction, Category, Transaction>(
                    sql,
                    (transaction, category) =>
                    {
                        transaction.Category = category;
                        return transaction;
                    },
                    new { UserId = userId },
                    splitOn: "Id"
                );
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener transacciones: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsDapperAsync()
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DataBaseProvider.MySql => Primero.AllTransactionsMySql,
                    _ => throw new NotSupportedException("Provider no soportado")
                };

                return await _dapper.QueryAsync<Transaction>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener todas las transacciones con Dapper: {ex.Message}");
            }
        }

        public async Task<Transaction> GetTransactionByIdDapperAsync(int id)
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DataBaseProvider.MySql => Primero.TransactionByIdMySql,
                    _ => throw new NotSupportedException("Provider no soportado")
                };

                return await _dapper.QueryFirstOrDefaultAsync<Transaction>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<decimal> GetTotalBalanceByUserId(int userId)
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DataBaseProvider.MySql => Primero.GetTotalBalance,
                    _ => throw new NotSupportedException("Provider no soportado")
                };

                return await _dapper.ExecuteScalarAsync<decimal>(sql, new { UserId = userId });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<MonthlyBalanceDto> GetMonthlyBalanceDapperAsync(int userId, int month, int year)
        {
            try
            {
                var sqlTotals = _dapper.Provider switch
                {
                    DataBaseProvider.MySql => Primero.TotalesMySql,
                    _ => throw new NotSupportedException("Provider no soportado")
                };

                var sqlDetails = Primero.DetalleMySql;

                var parameters = new { UserId = userId, Month = month, Year = year };

                var balance = await _dapper.QueryFirstOrDefaultAsync<MonthlyBalanceDto>(sqlTotals, parameters);
                var details = await _dapper.QueryAsync<CategorySummaryDto>(sqlDetails, parameters);

                if (balance != null)
                {
                    balance.Details = details.ToList();
                }

                return balance ?? new MonthlyBalanceDto();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
