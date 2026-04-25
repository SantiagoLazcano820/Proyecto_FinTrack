using FinTrack.Core.Enum;
using FinTrack.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;

namespace FinTrack.Infraestructure.Repositories
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _config;
        private readonly string _mySqlConn;
        public DataBaseProvider Provider { get; }

        public DbConnectionFactory(IConfiguration config)
        {
            _config = config;
            _mySqlConn = _config.GetConnectionString("ConnectionMySql")
                ?? string.Empty;

            var providerStr = _config.GetSection("DatabaseProvider").Value
                ?? "MySql";

            Provider = providerStr.Equals("MySql", StringComparison.OrdinalIgnoreCase)
                ? DataBaseProvider.MySql
                : DataBaseProvider.SqlServer;
        }


        public IDbConnection CreateConnection()
        {
            return Provider switch
            {
                DataBaseProvider.MySql => new MySqlConnection(_mySqlConn)
            };
        }
    }
}
