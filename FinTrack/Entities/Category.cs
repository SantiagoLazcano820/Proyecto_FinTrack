using System.Collections.Generic;

namespace FinTrack.Core.Entities;

/// <summary>
/// Representa una Categoría en el sistema.
/// </summary>
/// <remarks>
/// Esta entidad almacena la información principal de las categorías utilizadas para organizar y clasificar las transacciones financieras de los usuarios.
/// </remarks>
public partial class Category : BaseEntity
{
    /// <summary>
    /// Identificador único del usuario dueño de la categoría.
    /// </summary>
    /// <example>12</example>
    public int UserId { get; set; }

    /// <summary>
    /// Nombre descriptivo de la categoría.
    /// </summary>
    /// <example>Alimentación</example>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Descripción extendida sobre qué tipo de gastos o ingresos incluye esta categoría.
    /// </summary>
    /// <example>Gastos relacionados con supermercados, restaurantes y comida a domicilio.</example>
    public string? Description { get; set; }

    /// <summary>
    /// Estado de activación de la categoría (1 para Activo, 0 para Inactivo).
    /// </summary>
    /// <example>1</example>
    public ulong IsActive { get; set; }

    /// <summary>
    /// Objeto virtual que representa la relación con el [Usuario] dueño de esta categoría.
    /// </summary>
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// Colección de las [Transacciones] financieras asociadas de forma directa a esta categoría.
    /// </summary>
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}