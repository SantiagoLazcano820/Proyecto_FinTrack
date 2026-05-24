namespace FinTrack.Core.DTOs
{
    /// <summary>
    /// Representa una [Entidad] de transferencia de datos (DTO) para los usuarios del sistema.
    /// </summary>
    /// <remarks>
    /// Esta entidad almacena la información principal de un usuario
    /// y es utilizada para exponer datos de perfil o autenticación de forma segura a través de la API.
    /// </remarks>
    public class UserDto
    {
        /// <summary>
        /// Identificador único del usuario.
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Identificador único del rol asociado al usuario.
        /// </summary>
        /// <example>2</example>
        public int RoleId { get; set; }

        /// <summary>
        /// Nombre(s) del usuario.
        /// </summary>
        /// <example>Santiago</example>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Apellidos del usuario.
        /// </summary>
        /// <example>Lazcano</example>
        public string LastName { get; set; } = null!;

        /// <summary>
        /// Dirección de correo electrónico del usuario.
        /// </summary>
        /// <example>santiago.lazcano@ucb.edu.bo</example>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Contraseña del usuario (opcional, generalmente utilizada para flujos de registro o login).
        /// </summary>
        /// <example>P@ssw0rd2026</example>
        public string? Password { get; set; }
    }
}