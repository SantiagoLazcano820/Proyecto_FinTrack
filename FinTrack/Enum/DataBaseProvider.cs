namespace FinTrack.Core.Enum
{
    /// <summary>
    /// Representa un [Enum] para los proveedores de bases de datos soportados.
    /// </summary>
    /// <remarks>
    /// Utilizado en la configuración del núcleo de persistencia para alternar dinámicamente el motor de base de datos de FinTrack.
    /// </remarks>
    public enum DataBaseProvider
    {
        /// <summary>
        /// Motor de base de datos Microsoft SQL Server.
        /// </summary>
        SqlServer,

        /// <summary>
        /// Motor de base de datos MySQL Server.
        /// </summary>
        MySql
    }
}