namespace FinTrack.Core.DTOs
{
    /// <summary>
    /// Representa una [Entidad] de transferencia de datos (DTO) para los roles de usuario.
    /// </summary>
    /// <remarks>
    /// Esta entidad almacena la información principal de los roles asignables del sistema 
    /// y se usa para el control de accesos y permisos.
    /// </remarks>
    public class RoleDto
    {
        /// <summary>
        /// Identificador único del rol.
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Nombre identificador del rol en el sistema.
        /// </summary>
        /// <example>Usuario Estándar</example>
        public string Name { get; set; } = null!;
    }
}