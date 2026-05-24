using FinTrack.Core.Enum;

namespace FinTrack.Core.Entities;

/// <summary>
/// Representa una Credencial de Autenticación en el sistema mediante la [Entidad] Security.
/// </summary>
/// <remarks>
/// Esta entidad almacena de forma aislada y segura los datos de acceso, nombres y roles del personal o usuarios administradores del API.
/// </remarks>
public partial class Security : BaseEntity
{
    /// <summary>
    /// Nombre de usuario o identificador de inicio de sesión (Login).
    /// </summary>
    /// <example>santiago.admin</example>
    public string Login { get; set; } = null!;

    /// <summary>
    /// Contraseña codificada o credencial secreta de acceso.
    /// </summary>
    /// <example>hashed_security_string_2026</example>
    public string Password { get; set; } = null!;

    /// <summary>
    /// Nombre descriptivo completo de la persona asociada a esta credencial.
    /// </summary>
    /// <example>Santiago Lazcano</example>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Tipo de rol de seguridad asignado (mapeado dinámicamente desde un enumerador).
    /// </summary>
    /// <example>Administrator</example>
    public RoleType Role { get; set; }
}