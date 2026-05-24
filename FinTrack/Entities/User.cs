using System.Collections.Generic;

namespace FinTrack.Core.Entities;

/// <summary>
/// Representa un Usuario en el sistema.
/// </summary>
/// <remarks>
/// Esta entidad almacena los datos de perfil, credenciales de acceso primarias y relaciones financieras globales del usuario.
/// </remarks>
public partial class User : BaseEntity
{
    /// <summary>
    /// Identificador del rol asignado para definir los permisos del usuario.
    /// </summary>
    /// <example>2</example>
    public int RoleId { get; set; }

    /// <summary>
    /// Nombre(s) del usuario.
    /// </summary>
    /// <example>Santiago</example>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Apellido(s) del usuario.
    /// </summary>
    /// <example>Lazcano</example>
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Dirección de correo electrónico única para el inicio de sesión.
    /// </summary>
    /// <example>ejemplo@gmail.com</example>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Contraseña codificada de acceso a la cuenta.
    /// </summary>
    /// <example>hashed_password_string_example</example>
    public string Password { get; set; } = null!;

    /// <summary>
    /// Define si la cuenta de usuario se encuentra activa (1 para Activo, 0 para Suspendido/Inactivo).
    /// </summary>
    /// <example>1</example>
    public ulong IsActive { get; set; }

    /// <summary>
    /// Objeto virtual que representa la relación con el [Rol] de seguridad asignado al usuario.
    /// </summary>
    public virtual Role Role { get; set; } = null!;

    /// <summary>
    /// Colección de todas las [Transacciones] financieras asociadas que pertenecen a este usuario.
    /// </summary>
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    /// <summary>
    /// Colección de todas las [Categorías] personalizadas creadas u organizadas por este usuario.
    /// </summary>
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}