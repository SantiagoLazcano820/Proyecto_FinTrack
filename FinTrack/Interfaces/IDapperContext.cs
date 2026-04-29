using FinTrack.Core.Enum;
using System.Data;

namespace FinTrack.Core.Interfaces
{
    public interface IDapperContext
    {
        DataBaseProvider Provider { get; }

        Task<IEnumerable<T>> QueryAsync<T>(
            string sql,
            object? param = null,
            CommandType commandType = CommandType.Text);

        Task<T?> QueryFirstOrDefaultAsync<T>(
            string sql,
            object? param = null,
            CommandType commandType = CommandType.Text);

        Task<int> ExecuteAsync(
            string sql,
            object? param = null,
            CommandType commandType = CommandType.Text);

        Task<T> ExecuteScalarAsync<T>(
            string sql,
            object? param = null,
            CommandType commandType = CommandType.Text
            );

        void SetAmbientConnection(
            IDbConnection conn,
            IDbTransaction? tx);

        void ClearAmbientConnection();

        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(
            string sql,
            Func<TFirst, TSecond, TReturn> map,
            object param = null,
            string splitOn = "Id");
    }
}
