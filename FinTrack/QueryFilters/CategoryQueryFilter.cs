using Swashbuckle.AspNetCore.Annotations;

namespace FinTrack.Core.QueryFilters
{
    /// <summary>
    /// Representa la [Entidad] de filtros acumulativos para la gestión de categorías.
    /// </summary>
    /// <remarks>
    /// Provee los criterios selectivos necesarios para retornar de forma limpia y ordenada las carpetas organizacionales 
    /// de presupuestos de un usuario determinado.
    /// </remarks>
    public class CategoryQueryFilter : PaginationQueryFilter
    {
        /// <summary>
        /// Identificador único del usuario dueño de las categorías.
        /// </summary>
        /// <example>12</example>
        [SwaggerSchema("ID del usuario para filtrar sus categorías personalizadas", Nullable = true)]
        public int? UserId { get; set; }

        /// <summary>
        /// Nombre de la categoría.
        /// </summary>
        /// <example>Alimentación</example>
        [SwaggerSchema("Nombre o coincidencia parcial de la categoría para la búsqueda", Nullable = true)]
        public string? Name { get; set; }
    }
}