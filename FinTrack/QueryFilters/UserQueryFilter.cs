using Swashbuckle.AspNetCore.Annotations;

namespace FinTrack.Core.QueryFilters
{
    /// <summary>
    /// Representa la [Entidad] de filtros acumulativos para la búsqueda de usuarios.
    /// </summary>
    /// <remarks>
    /// Hereda los parámetros lógicos de paginación y añade campos opcionales para la localización estructurada 
    /// de perfiles de usuario basados en nombres, apellidos o correos.
    /// </remarks>
    public class UserQueryFilter : PaginationQueryFilter
    {
        /// <summary>
        /// Nombre del usuario.
        /// </summary>
        /// <example>Santiago</example>
        [SwaggerSchema("Nombre o texto de coincidencia para buscar usuarios", Nullable = true)]
        public string? Name { get; set; }

        /// <summary>
        /// Apellido del usuario.
        /// </summary>
        /// <example>Lazcano</example>
        [SwaggerSchema("Apellido paterno o materno del usuario para el filtro", Nullable = true)]
        public string? LastName { get; set; }

        /// <summary>
        /// Correo electrónico institucional o personal del usuario.
        /// </summary>
        /// <example>santiago.lazcano@ucb.edu.bo</example>
        [SwaggerSchema("Dirección de correo electrónico exacta o parcial", Nullable = true)]
        public string? Email { get; set; }
    }
}