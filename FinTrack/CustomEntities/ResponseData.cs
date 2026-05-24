using System.Net;
using System.Text.Json.Serialization;

namespace FinTrack.Core.CustomEntities
{
    /// <summary>
    /// Representa una [Entidad] base estructurada para envolver los datos devueltos por los servicios de la aplicación.
    /// </summary>
    /// <remarks>
    /// Esta clase permite transportar unificados los elementos paginados de datos generales del sistema, 
    /// los mensajes del negocio y el código de estado HTTP correspondiente para su fácil procesamiento.
    /// </remarks>
    public class ResponseData
    {
        /// <summary>
        /// Estructura interna de datos que almacena la información de paginación de la consulta.
        /// </summary>
        public PagedList<object> Pagination { get; set; }

        /// <summary>
        /// Arreglo o colección de mensajes de control, errores o advertencias del procesamiento.
        /// </summary>
        public Message[] Messages { get; set; }

        /// <summary>
        /// Código de estado HTTP de negocio relacionado con el resultado final del servicio. Ignorado en la serialización JSON.
        /// </summary>
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }
    }
}