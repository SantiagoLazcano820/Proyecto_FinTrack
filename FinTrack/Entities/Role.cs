using System.Collections.Generic;

namespace FinTrack.Core.Entities;

/// <summary>
/// Representa un Rol de usuario en el sistema.
/// </summary>
/// <remarks>
/// Esta entidad define el nivel de acceso, permisos y el tipo de cuenta que posee un usuario dentro de la plataforma.
/// </remarks>
public partial class Role : BaseEntity
{
    /// <summary>
    /// Nombre del rol asignado dentro del sistema.
    /// </summary>
    /// <example>Usuario Estándar</example>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Colección de los [Usuarios] del sistema que tienen asignado este rol específico.
    /// </summary>
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}