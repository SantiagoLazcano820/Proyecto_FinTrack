namespace FinTrack.Core.Enum
{
    /// <summary>
    /// Representa un [Enum] para clasificar los tipos de roles de seguridad del sistema.
    /// </summary>
    /// <remarks>
    /// Define los niveles jerárquicos de accesos permitidos para los registros de la tabla Security de FinTrack.
    /// </remarks>
    public enum RoleType
    {
        /// <summary>
        /// Rol con privilegios totales de administración y control del ecosistema.
        /// </summary>
        Admin,

        /// <summary>
        /// Rol de usuario consumidor estándar para la gestión de sus finanzas personales.
        /// </summary>
        StandardUser
    }
}