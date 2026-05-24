using FinTrack.Core.Enum;
using System.Data;

namespace FinTrack.Core.Interfaces
{
    /// <summary>
    /// Define el contrato de abstracción para la [Entidad] o contexto personalizado de ejecución con Dapper.
    /// </summary>
    /// <remarks>
    /// Encapsula las operaciones comunes del micro-ORM Dapper permitiendo ejecutar consultas asíncronas seguras, 
    /// gestionar mapeos de múltiples objetos y controlar transacciones concurrentes de bases de datos.
    /// </remarks>
    public interface IDapperContext
    {
        /// <summary>
        /// Obtiene el proveedor actual de base de datos relacional inyectado en el sistema.
        /// </summary>
        DataBaseProvider Provider { get; }

        /// <summary>
        /// Ejecuta una consulta SQL asíncrona que retorna una colección de múltiples registros o resultados.
        /// </summary>
        /// <typeparam name="T">El tipo de objeto esperado como salida de mapeo.</typeparam>
        /// <param name="sql">Sentencia o comando SQL a ejecutar en el servidor.</param>
        /// <param name="param">Objeto anónimo con los parámetros de la consulta (evita SQL Injection).</param>
        /// <param name="commandType">Tipo de comando interpretado (Text o StoredProcedure).</param>
        /// <returns>Una lista o colección enumerable con los datos mapeados a tipo <typeparamref name="T"/>.</returns>
        Task<IEnumerable<T>> QueryAsync<T>(
            string sql,
            object? param = null,
            CommandType commandType = CommandType.Text);

        /// <summary>
        /// Ejecuta una consulta SQL asíncrona que retorna únicamente el primer registro encontrado o un valor nulo.
        /// </summary>
        /// <typeparam name="T">El tipo de entidad o clase esperada.</typeparam>
        /// <param name="sql">Sentencia o comando SQL de selección.</param>
        /// <param name="param">Objeto anónimo con los parámetros de la consulta.</param>
        /// <param name="commandType">Tipo de comando interpretado (Text o StoredProcedure).</param>
        /// <returns>El primer registro de tipo <typeparamref name="T"/> encontrado, o null si la consulta no arrojó resultados.</returns>
        Task<T?> QueryFirstOrDefaultAsync<T>(
            string sql,
            object? param = null,
            CommandType commandType = CommandType.Text);

        /// <summary>
        /// Ejecuta de forma asíncrona comandos SQL de escritura (INSERT, UPDATE, DELETE) que no devuelven registros.
        /// </summary>
        /// <param name="sql">Sentencia o comando SQL de alteración o comandos de manipulación DML.</param>
        /// <param name="param">Objeto anónimo con los parámetros necesarios del comando.</param>
        /// <param name="commandType">Tipo de comando interpretado (Text o StoredProcedure).</param>
        /// <returns>El número total de filas o registros que fueron afectados en la base de datos.</returns>
        Task<int> ExecuteAsync(
            string sql,
            object? param = null,
            CommandType commandType = CommandType.Text);

        /// <summary>
        /// Ejecuta una consulta SQL asíncrona y devuelve la primera columna de la primera fila en el tipo indicado.
        /// </summary>
        /// <typeparam name="T">El tipo primitivo o clase escalar esperado (ej. int, decimal, string).</typeparam>
        /// <param name="sql">Sentencia SQL típicamente de agregación (COUNT, SUM, MAX).</param>
        /// <param name="param">Objeto anónimo con los parámetros requeridos.</param>
        /// <param name="commandType">Tipo de comando interpretado (Text o StoredProcedure).</param>
        /// <returns>El valor escalar de tipo <typeparamref name="T"/> devuelto por la base de datos.</returns>
        Task<T> ExecuteScalarAsync<T>(
            string sql,
            object? param = null,
            CommandType commandType = CommandType.Text
            );

        /// <summary>
        /// Establece y asocia de forma explícita el entorno o contexto de conexión y transaccionalidad actual.
        /// </summary>
        /// <remarks>
        /// Permite sincronizar la misma conexión del Unit of Work con Dapper dentro del ámbito ambiental vigente.
        /// </remarks>
        /// <param name="conn">Instancia de conexión activa a la base de datos.</param>
        /// <param name="tx">Instancia de transacción activa de base de datos o nulo si no aplica.</param>
        void SetAmbientConnection(
            IDbConnection conn,
            IDbTransaction? tx);

        /// <summary>
        /// Libera, limpia y desvincula las referencias de conexión ambiental previamente asignadas al contexto.
        /// </summary>
        void ClearAmbientConnection();

        /// <summary>
        /// Ejecuta una consulta SQL asíncrona de mapeo múltiple o relacional combinando dos entidades en un solo modelo.
        /// </summary>
        /// <typeparam name="TFirst">El tipo de la primera entidad en el mapeo (ej. Transaction).</typeparam>
        /// <typeparam name="TSecond">El tipo de la segunda entidad relacionada en el mapeo (ej. Category).</typeparam>
        /// <typeparam name="TReturn">El tipo del objeto resultante unificado de la iteración de mapeo.</typeparam>
        /// <param name="sql">Sentencia SQL que incluye operaciones de cruce de datos (JOIN).</param>
        /// <param name="map">Expresión o función lambda que define el flujo y orden de mapeo manual entre los objetos.</param>
        /// <param name="param">Objeto con parámetros requeridos por la sentencia SQL.</param>
        /// <param name="splitOn">Nombre de la columna pivote en la selección SQL para dividir las entidades del JOIN en el pipeline.</param>
        /// <returns>Una colección de objetos de tipo <typeparamref name="TReturn"/> completamente relacionados.</returns>
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(
            string sql,
            Func<TFirst, TSecond, TReturn> map,
            object param = null,
            string splitOn = "Id");
    }
}