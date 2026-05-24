using FinTrack.Core.Entities;

namespace FinTrack.Core.Interfaces
{
    /// <summary>
    /// Define el contrato especializado de repositorio para la [Entidad] de gestión de usuarios.
    /// </summary>
    /// <remarks>
    /// Extiende las capacidades del repositorio base implementando consultas de alto rendimiento optimizadas con Dapper.
    /// </remarks>
    public interface IUserRepository : IBaseRepository<User>
    {
        /// <summary>
        /// Recupera un usuario utilizando su dirección de correo electrónico a través de consultas rápidas con Dapper.
        /// </summary>
        /// <param name="email">Dirección de correo electrónico del usuario.</param>
        /// <returns>El objeto <see cref="User"/> asociado al correo proporcionado.</returns>
        Task<User> GetUserByEmailDapperAsync(string email);

        /// <summary>
        /// Recupera la lista completa de usuarios registrados mapeados eficientemente con Dapper.
        /// </summary>
        /// <returns>Una colección de objetos <see cref="User"/>.</returns>
        Task<IEnumerable<User>> GetAllUsersDapperAsync();

        /// <summary>
        /// Obtiene un usuario específico mediante su identificador único utilizando Dapper.
        /// </summary>
        /// <param name="id">Identificador del usuario.</param>
        /// <returns>El objeto <see cref="User"/> que coincide con el identificador.</returns>
        Task<User> GetUserByIdDapperAsync(int id);
    }
}