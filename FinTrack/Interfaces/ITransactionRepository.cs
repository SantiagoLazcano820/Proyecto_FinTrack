using FinTrack.Core.DTOs;
using FinTrack.Core.Entities;

namespace FinTrack.Core.Interfaces
{
    /// <summary>
    /// Define el contrato especializado de repositorio para la [Entidad] de transacciones financieras.
    /// </summary>
    /// <remarks>
    /// Expone métodos para el análisis financiero avanzado y búsquedas de transacciones utilizando el micro-ORM Dapper para máxima velocidad de procesamiento.
    /// </remarks>
    public interface ITransactionRepository : IBaseRepository<Transaction>
    {
        /// <summary>
        /// Obtiene todas las transacciones financieras que pertenecen de manera exclusiva a un usuario específico.
        /// </summary>
        /// <param name="userId">Identificador del usuario propietario.</param>
        /// <returns>Una colección de transacciones financieras.</returns>
        Task<IEnumerable<Transaction>> GetTransactionsByUserIdDapperAsync(int userId);

        /// <summary>
        /// Recupera la totalidad de transacciones registradas globalmente en el sistema mediante Dapper.
        /// </summary>
        /// <returns>Una colección de todas las transacciones.</returns>
        Task<IEnumerable<Transaction>> GetAllTransactionsDapperAsync();

        /// <summary>
        /// Recupera una transacción individual según su identificador utilizando Dapper.
        /// </summary>
        /// <param name="id">Identificador único de la transacción.</param>
        /// <returns>La transacción financiera de tipo <see cref="Transaction"/> encontrada.</returns>
        Task<Transaction> GetTransactionByIdDapperAsync(int id);

        /// <summary>
        /// Calcula la sumatoria o balance de fondos general actual de un usuario (Ingresos totales menos Egresos totales).
        /// </summary>
        /// <param name="userId">Identificador único del usuario consultado.</param>
        /// <returns>El monto decimal con el balance financiero acumulado.</returns>
        Task<decimal> GetTotalBalanceByUserId(int userId);

        /// <summary>
        /// Recupera y procesa el resumen del balance mensual estructurado de un usuario para un periodo determinado con Dapper.
        /// </summary>
        /// <param name="userId">Identificador único del usuario.</param>
        /// <param name="month">Número de mes evaluado (1 al 12).</param>
        /// <param name="year">Año de la consulta.</param>
        /// <returns>Un objeto de transferencia de datos <see cref="MonthlyBalanceDto"/> con métricas mensuales e indicadores de déficit.</returns>
        Task<MonthlyBalanceDto> GetMonthlyBalanceDapperAsync(int userId, int month, int year);
    }
}