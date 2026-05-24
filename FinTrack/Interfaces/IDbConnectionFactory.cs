using FinTrack.Core.Enum;
using System.Data;

namespace FinTrack.Core.Interfaces
{
    /// <summary>
    /// Define el contrato técnico para una fábrica de conexiones de bases de datos ([Entidad] Factory).
    /// </summary>
    /// <remarks>
    /// Abstrae la instanciación de conexiones hacia los distintos proveedores de bases de datos relacionales configurados en el sistema de manera transparente.
    /// </remarks>
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// Obtiene el proveedor de base de datos activo actualmente configurado en la solución.
        /// </summary>
        DataBaseProvider Provider { get; }

        /// <summary>
        /// Inicializa, construye y devuelve una nueva instancia de conexión a la base de datos correspondiente.
        /// </summary>
        /// <returns>Un objeto que implementa <see cref="IDbConnection"/> listo para interactuar mediante Dapper o ADO.NET.</returns>
        IDbConnection CreateConnection();
    }
}