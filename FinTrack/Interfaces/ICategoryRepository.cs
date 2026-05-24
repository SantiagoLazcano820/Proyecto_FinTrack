using FinTrack.Core.Entities;

namespace FinTrack.Core.Interfaces
{
    /// <summary>
    /// Define el contrato especializado de repositorio para la [Entidad] de categorías.
    /// </summary>
    /// <remarks>
    /// Extiende el repositorio genérico base ofreciendo consultas de alto rendimiento gestionadas con Dapper para la segmentación de transacciones.
    /// </remarks>
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        /// <summary>
        /// Recupera todas las categorías financieras que pertenecen a un usuario específico mediante Dapper.
        /// </summary>
        /// <param name="userId">Identificador único del usuario propietario.</param>
        /// <returns>Una colección de categorías financieras de tipo <see cref="Category"/>.</returns>
        Task<IEnumerable<Category>> GetCategoriesByUserIdDapperAsync(int userId);

        /// <summary>
        /// Recupera la lista completa de todas las categorías mapeadas con Dapper globalmente.
        /// </summary>
        /// <returns>Una colección general de objetos <see cref="Category"/>.</returns>
        Task<IEnumerable<Category>> GetAllCategoriesDapperAsync();

        /// <summary>
        /// Obtiene una categoría individual a través de su identificador único utilizando Dapper.
        /// </summary>
        /// <param name="id">Identificador único de la categoría buscada.</param>
        /// <returns>La instancia de la [Entidad] <see cref="Category"/> correspondiente.</returns>
        Task<Category> GetCategoryByIdDapperAsync(int id);
    }
}