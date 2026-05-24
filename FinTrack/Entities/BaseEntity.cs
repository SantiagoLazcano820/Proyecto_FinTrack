namespace FinTrack.Core.Entities
{
    /// <summary>
    /// Clase base para todas las entidades del sistema.
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Identificador único de la entidad en la base de datos.
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }
    }
}