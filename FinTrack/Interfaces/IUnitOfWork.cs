using FinTrack.Core.Entities;
using System.Data;

namespace FinTrack.Core.Interfaces
{
    /// <summary>
    /// Define el contrato de arquitectura para la [Entidad] o patrón Unidad de Trabajo (Unit of Work).
    /// </summary>
    /// <remarks>
    /// Centraliza el acceso a todos los repositorios y unifica la confirmación de transacciones transaccionales ACID de forma síncrona o asíncrona.
    /// </remarks>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Acceso al repositorio especializado de transacciones financieras.
        /// </summary>
        ITransactionRepository TransactionRepository { get; }

        /// <summary>
        /// Acceso al repositorio especializado de usuarios de la aplicación.
        /// </summary>
        IUserRepository UserRepository { get; }

        /// <summary>
        /// Acceso al repositorio especializado de categorías.
        /// </summary>
        ICategoryRepository CategoryRepository { get; }

        /// <summary>
        /// Acceso al repositorio especializado de credenciales y cuentas de seguridad de FinTrack.
        /// </summary>
        ISecurityRepository SecurityRepository { get; }

        /// <summary>
        /// Confirma y guarda todos los cambios realizados en los repositorios de manera persistente y síncrona.
        /// </summary>
        /// 
        void SaveChanges();

        /// <summary>
        /// Confirma y guarda todos los cambios de forma asíncrona.
        /// </summary>
        Task SaveChangesAsync();

        /// <summary>
        /// Inicia formalmente un bloque de transacción asíncrono para operaciones complejas de negocio (ACID).
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Aplica y consolida definitivamente los cambios de la transacción actual en la base de datos.
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// Deshace o revierte todos los cambios de la transacción actual si ocurrió alguna anomalía.
        /// </summary>
        Task RollbackAsync();

        /// <summary>
        /// Obtiene la interfaz de conexión subyacente para realizar consultas personalizadas directas de base de datos.
        /// </summary>
        /// <returns>La conexión de base de datos de tipo <see cref="IDbConnection"/>.</returns>
        IDbConnection GetDbConnection();

        /// <summary>
        /// Obtiene la transacción de base de datos activa o nulo si no se ha iniciado un entorno transaccional.
        /// </summary>
        /// <returns>La transacción de base de datos <see cref="IDbTransaction"/> o null.</returns>
        IDbTransaction? GetDbTransaction();
    }
}