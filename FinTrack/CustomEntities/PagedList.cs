namespace FinTrack.Core.CustomEntities
{
    /// <summary>
    /// Representa una [Entidad] genérica para el manejo de listas de datos paginadas.
    /// </summary>
    /// <remarks>
    /// Hereda de una lista estándar y añade propiedades que calculan automáticamente los metadatos de paginación
    /// requeridos por los clientes de la API, optimizando la manipulación de colecciones extensas.
    /// </remarks>
    /// <typeparam name="T">El tipo de objeto de los elementos contenidos en la lista.</typeparam>
    public class PagedList<T> : List<T>
    {
        #region Atributos
        /// <summary>
        /// Número de la página actual recuperada en la consulta.
        /// </summary>
        /// <example>1</example>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Cantidad total de páginas disponibles basadas en el tamaño de página y el total de registros.
        /// </summary>
        /// <example>5</example>
        public int TotalPages { get; set; }

        /// <summary>
        /// Cantidad máxima de registros permitidos o devuelvos en una sola página.
        /// </summary>
        /// <example>10</example>
        public int PageSize { get; set; }

        /// <summary>
        /// Cantidad total de registros existentes en la base de datos que coinciden con los filtros aplicados.
        /// </summary>
        /// <example>50</example>
        public int TotalCount { get; set; }
        #endregion

        #region Propiedades
        /// <summary>
        /// Indica si existe una página previa con respecto a la página actual.
        /// </summary>
        /// <example>false</example>
        public bool HasPreviousPage => CurrentPage > 1;

        /// <summary>
        /// Indica si existe una página posterior con respecto a la página actual.
        /// </summary>
        /// <example>true</example>
        public bool HasNextPage => CurrentPage < TotalPages;

        /// <summary>
        /// Devuelve el número correspondiente a la siguiente página, o nulo si no existe.
        /// </summary>
        /// <example>2</example>
        public int? NextPageNumber => HasNextPage ? CurrentPage + 1 : null;

        /// <summary>
        /// Devuelve el número correspondiente a la página anterior, o nulo si no existe.
        /// </summary>
        /// <example>null</example>
        public int? PreviousPageNumber => HasPreviousPage ? CurrentPage - 1 : null;
        #endregion

        public PagedList(List<T> items,
            int count,
            int pageNumber,
            int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public static PagedList<T> Create
            (IEnumerable<T> source,
            int pageNumber,
            int pageSize)
        {
            var count = source.Count();
            var items = source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToList();

            return new PagedList<T>
                (items, count, pageNumber, pageSize);
        }
    }
}