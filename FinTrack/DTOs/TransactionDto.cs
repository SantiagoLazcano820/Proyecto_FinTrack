namespace FinTrack.Core.DTOs
{
    /// <summary>
    /// Representa una [Entidad] de transferencia de datos (DTO) para una transacción financiera.
    /// </summary>
    /// <remarks>
    /// Esta entidad almacena la información principal de un movimiento financiero (ingreso o egreso) 
    /// y es utilizada para las operaciones de visualización y persistencia.
    /// </remarks>
    public class TransactionDto
    {
        /// <summary>
        /// Identificador único de la transacción.
        /// </summary>
        /// <example>101</example>
        public int Id { get; set; }

        /// <summary>
        /// Identificador único del usuario dueño de la transacción.
        /// </summary>
        /// <example>1</example>
        public int UserId { get; set; }

        /// <summary>
        /// Identificador único de la categoría a la que pertenece el movimiento.
        /// </summary>
        /// <example>3</example>
        public int CategoryId { get; set; }

        /// <summary>
        /// Monto económico de la transacción.
        /// </summary>
        /// <example>150.50</example>
        public decimal Amount { get; set; }

        /// <summary>
        /// Tipo de transacción (por ejemplo: Ingreso, Egreso).
        /// </summary>
        /// <example>Egreso</example>
        public string Type { get; set; } = null!;

        /// <summary>
        /// Fecha en la que se realizó la transacción en formato de texto.
        /// </summary>
        /// <example>2026-05-23</example>
        public string Date { get; set; } = null!;

        /// <summary>
        /// Descripción u observación breve sobre el origen o destino del dinero.
        /// </summary>
        /// <example>Compra de víveres para el mes</example>
        public string? Description { get; set; }
    }
}