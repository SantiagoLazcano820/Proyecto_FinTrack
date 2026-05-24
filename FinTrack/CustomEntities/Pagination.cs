namespace FinTrack.Core.CustomEntities
{
    /// <summary>
    /// Representa la [Entidad] con el modelo de metadatos de paginación para las respuestas de la API.
    /// </summary>
    /// <remarks>
    /// Encapsula las propiedades clave calculadas a partir de una lista paginada para ser expuestas de forma
    /// limpia y serializable en la capa de presentación de los controladores.
    /// </remarks>
    public class Pagination
    {
        /// <summary>
        /// Cantidad total de registros que satisfacen los criterios de búsqueda de la consulta.
        /// </summary>
        /// <example>120</example>
        public int TotalCount { get; set; }

        /// <summary>
        /// Tamaño o número de elementos configurados por página.
        /// </summary>
        /// <example>25</example>
        public int PageSize { get; set; }

        /// <summary>
        /// Número del índice de la página actual de datos.
        /// </summary>
        /// <example>2</example>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Número total calculado de páginas para la consulta.
        /// </summary>
        /// <example>5</example>
        public int TotalPages { get; set; }

        /// <summary>
        /// Indica si hay más registros en páginas posteriores.
        /// </summary>
        /// <example>true</example>
        public bool HasNextPage { get; set; }

        /// <summary>
        /// Indica si existen registros en páginas anteriores de la paginación.
        /// </summary>
        /// <example>true</example>
        public bool HasPreviousPage { get; set; }

        public Pagination()
        {

        }

        public Pagination(PagedList<object> lista)
        {
            TotalCount = lista.TotalCount;
            PageSize = lista.PageSize;
            CurrentPage = lista.CurrentPage;
            TotalPages = lista.TotalPages;
            HasNextPage = lista.HasNextPage;
            HasPreviousPage = lista.HasPreviousPage;
        }
    }
}