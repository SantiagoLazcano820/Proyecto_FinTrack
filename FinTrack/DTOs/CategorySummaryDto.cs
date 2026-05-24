namespace FinTrack.Core.DTOs
{
    /// <summary>
    /// Representa una [Entidad] de transferencia de datos (DTO) para el resumen agrupado de una categoría.
    /// </summary>
    /// <remarks>
    /// Esta entidad almacena los totales acumulados por cada categoría en las pantallas de balances 
    /// o reportes gráficos de la aplicación móvil.
    /// </remarks>
    public class CategorySummaryDto
    {
        /// <summary>
        /// Nombre descriptivo de la categoría.
        /// </summary>
        /// <example>Alimentación</example>
        public string CategoryName { get; set; } = null!;

        /// <summary>
        /// Suma total acumulada de las transacciones asociadas a esta categoría durante el periodo.
        /// </summary>
        /// <example>1250.00</example>
        public decimal Total { get; set; }

        /// <summary>
        /// Tipo de transacciones que agrupa esta categoría (Ingreso / Egreso).
        /// </summary>
        /// <example>Egreso</example>
        public string Type { get; set; } = null!;
    }
}