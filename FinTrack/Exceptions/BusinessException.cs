using System.Net;

namespace FinTrack.Core.Exceptions
{
    /// <summary>
    /// Representa una [Entidad] de excepción personalizada para controlar los errores de lógica de negocio en el sistema.
    /// </summary>
    /// <remarks>
    /// Esta clase extiende de la clase base Exception y permite capturar de forma controlada fallos en las reglas de negocio, 
    /// asignándoles un código de estado HTTP y detalles adicionales para su posterior serialización en la respuesta global de la API.
    /// </remarks>
    public class BusinessException : Exception
    {
        /// <summary>
        /// Código de estado HTTP correspondiente a la regla de negocio que fue vulnerada o no encontrada.
        /// </summary>
        /// <example>404</example>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Objeto opcional con el desglose técnico o metadatos adicionales explicativos sobre la causa del fallo de negocio.
        /// </summary>
        /// <example>{"Motivo": "Saldo insuficiente para completar la transacción financiera."}</example>
        public object? Details { get; }

        public BusinessException(string message,
            HttpStatusCode statusCode = HttpStatusCode.BadRequest,
            object? details = null) : base(message)
        {
            StatusCode = statusCode;
            Details = details;
        }
    }
}