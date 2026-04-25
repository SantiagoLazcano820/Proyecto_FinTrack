using FinTrack.Core.Enum;
using System.Data;

namespace FinTrack.Core.Interfaces
{
    public interface IDbConnectionFactory
    {
        DataBaseProvider Provider { get; }
        IDbConnection CreateConnection();
    }
}
