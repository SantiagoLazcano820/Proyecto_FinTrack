using Swashbuckle.AspNetCore.Annotations;

namespace FinTrack.Core.QueryFilters
{
    /// <summary>
    /// Representa la [Entidad] base de abstracción para los filtros de paginación en el sistema.
    /// </summary>
    /// <remarks>
    /// Esta clase abstracta define las propiedades elementales requeridas para segmentar y paginar 
    /// las colecciones de datos enviadas por el cliente a los controladores de la API.
    /// </remarks>
    public abstract class PaginationQueryFilter
    {
        /// <summary>
        /// Número de registros por página.
        /// </summary>
        /// <example>10</example>
        [SwaggerSchema("Número de registros devueltos por página (Por defecto: 10)", Nullable = false)]
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Número de la página actual que se desea consultar.
        /// </summary>
        /// <example>1</example>
        [SwaggerSchema("Índice de la página que se va a recuperar, empezando en 1", Nullable = false)]
        public int PageNumber { get; set; } = 1;
    }
}