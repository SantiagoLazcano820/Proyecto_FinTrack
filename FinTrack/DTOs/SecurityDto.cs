using FinTrack.Core.Enum;

namespace FinTrack.Core.DTOs
{
    /// <summary>
    /// Representa una [Entidad] de transferencia de datos (DTO) para la gestión y registro de cuentas Security.
    /// </summary>
    /// <remarks>
    /// Utilizado para transferir de manera segura la información de credenciales sin exponer propiedades internas del ciclo de vida.
    /// </remarks>
    public class SecurityDto
    {
        /// <summary>
        /// Cuenta o nombre de usuario único para accesos corporativos.
        /// </summary>
        /// <example>santiago.admin</example>
        public string Login { get; set; } = null!;

        /// <summary>
        /// Contraseña en texto plano suministrada durante el registro para su posterior hash.
        /// </summary>
        /// <example>Myp@ss2026</example>
        public string Password { get; set; } = null!;

        /// <summary>
        /// Nombre de la persona titular de la cuenta de seguridad.
        /// </summary>
        /// <example>Santiago Lazcano</example>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Tipo de rol asignado (Administrator / Consumer).
        /// </summary>
        /// <example>Administrator</example>
        public RoleType? Role { get; set; }
    }
}