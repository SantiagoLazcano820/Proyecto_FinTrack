namespace FinTrack.Core.CustomEntities
{
    /// <summary>
    /// Representa una [Entidad] de respuesta de error en el sistema.
    /// </summary>
    /// <remarks>
    /// Esta entidad almacena los detalles técnicos y descriptivos cuando ocurre un error 
    /// en las peticiones HTTP y es utilizada para la persistencia o diagnóstico de fallos.
    /// </remarks>
    public class ErrorResponse
    {
        /// <summary>
        /// Código de estado HTTP de la respuesta de error.
        /// </summary>
        /// <example>400</example>
        public int Status { get; set; }

        /// <summary>
        /// Título corto o categoría general del error presentado.
        /// </summary>
        /// <example>Bad Request</example>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Mensaje detallado que explica la causa del error.
        /// </summary>
        /// <example>El ID de la entidad proporcionada no es válido o no coincide.</example>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Objeto opcional que contiene una lista o colección detallada de errores (ej. validaciones de FluentValidation).
        /// </summary>
        /// <example>{"Id": ["El campo Id debe ser mayor a cero."]}</example>
        public object? Errors { get; set; }

        /// <summary>
        /// Identificador único de seguimiento de la petición (Trace ID) útil para auditoría y logs.
        /// </summary>
        /// <example>00-4bf92f3577b34da6a3ce929d0e0e4736-00f067aa0ba902b7-01</example>
        public string? TraceId { get; set; }
    }
}
