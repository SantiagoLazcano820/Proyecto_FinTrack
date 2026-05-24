namespace FinTrack.Core.DTOs
{
    /// <summary>
    /// Representa una [Entidad] de transferencia de datos (DTO) para las categorías de transacciones.
    /// </summary>
    /// <remarks>
    /// Esta entidad almacena los campos requeridos para la creación, edición y catalogación de los tipos 
    /// de transacciones de un usuario.
    /// </remarks>
    public class CategoryDto
    {
        /// <summary>
        /// Identificador único de la categoría.
        /// </summary>
        /// <example>3</example>
        public int Id { get; set; }

        /// <summary>
        /// Identificador único del usuario propietario o creador de la categoría.
        /// </summary>
        /// <example>1</example>
        public int UserId { get; set; }

        /// <summary>
        /// Nombre asignado a la categoría de organización financiera.
        /// </summary>
        /// <example>Estudios Universitarios</example>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Breve nota aclaratoria sobre el tipo de gastos o ingresos que cubre la categoría.
        /// </summary>
        /// <example>Matrículas, libros, fotocopias y pensiones académicas</example>
        public string? Description { get; set; }

        /// <summary>
        /// Estado de habilitación de la categoría (1 = Activa, 0 = Inactiva).
        /// </summary>
        /// <example>1</example>
        public int IsActive { get; set; }
    }
}