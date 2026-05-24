using Swashbuckle.AspNetCore.Annotations;

namespace FinTrack.Core.QueryFilters
{
    /// <summary>
    /// Representa la [Entidad] de filtros acumulativos para el historial de transacciones financieras.
    /// </summary>
    /// <remarks>
    /// Permite a los clientes de la aplicación móvil realizar búsquedas avanzadas y reportes segmentando por usuario, 
    /// categorías, fechas específicas o tipos de flujos financieros.
    /// </remarks>
    public class TransactionQueryFilter : PaginationQueryFilter
    {
        /// <summary>
        /// Identificador único del usuario asociado a la transacción.
        /// </summary>
        /// <example>12</example>
        [SwaggerSchema("ID del usuario para obtener su historial financiero", Nullable = true)]
        public int? UserId { get; set; }

        /// <summary>
        /// Identificador de la categoría asignada a la transacción.
        /// </summary>
        /// <example>5</example>
        [SwaggerSchema("ID de la categoría (ej. Alimentación, Transporte) para segmentar el gasto", Nullable = true)]
        public int? CategoryId { get; set; }

        /// <summary>
        /// Tipo de movimiento financiero (Ingreso / Egreso).
        /// </summary>
        /// <example>Egreso</example>
        [SwaggerSchema("Tipo de transacción. Valores sugeridos: 'Ingreso', 'Egreso'", Nullable = true)]
        public string? Type { get; set; }

        /// <summary>
        /// Fecha de la transacción en formato de texto.
        /// </summary>
        /// <example>2026-05-23</example>
        [SwaggerSchema("Fecha exacta o periodo de la transacción (ej. '2026-05-16')", Nullable = true)]
        public string? Date { get; set; }

        /// <summary>
        /// Detalle o concepto de la transacción.
        /// </summary>
        /// <example>Compra de materiales de estudio</example>
        [SwaggerSchema("Palabras clave dentro de la descripción o concepto del movimiento", Nullable = true)]
        public string? Description { get; set; }
    }
}