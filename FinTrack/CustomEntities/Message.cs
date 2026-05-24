namespace FinTrack.Core.CustomEntities
{
    /// <summary>
    /// Representa una [Entidad] de mensaje informativo o de notificación en el sistema.
    /// </summary>
    /// <remarks>
    /// Esta entidad se utiliza dentro de las respuestas de la API para comunicar el estado de éxito,
    /// advertencia o error de los procesos de negocio ejecutados.
    /// </remarks>
    public class Message
    {
        /// <summary>
        /// Tipo de mensaje emitido por el sistema.
        /// </summary>
        /// <example>success</example>
        public string Type { get; set; }

        /// <summary>
        /// Descripción explícita o cuerpo informativo de la notificación.
        /// </summary>
        /// <example>Operación realizada con éxito.</example>
        public string Description { get; set; }
    }
}