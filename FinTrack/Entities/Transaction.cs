using System;

namespace FinTrack.Core.Entities;

/// <summary>
/// Representa una Transacción financiera en el sistema.
/// </summary>
/// <remarks>
/// Esta entidad registra de forma detallada cada movimiento de flujo de caja (ingresos o egresos) realizado por un usuario.
/// </remarks>
public partial class Transaction : BaseEntity
{
    /// <summary>
    /// Identificador único del usuario que registró el movimiento.
    /// </summary>
    /// <example>12</example>
    public int UserId { get; set; }

    /// <summary>
    /// Identificador de la categoría a la cual pertenece la transacción.
    /// </summary>
    /// <example>5</example>
    public int CategoryId { get; set; }

    /// <summary>
    /// Monto o valor monetario de la transacción.
    /// </summary>
    /// <example>150.50</example>
    public decimal Amount { get; set; }

    /// <summary>
    /// Tipo de flujo financiero aplicado.
    /// </summary>
    /// <example>Egreso</example>
    public string Type { get; set; } = null!;

    /// <summary>
    /// Fecha y hora exacta en la que se efectuó el movimiento.
    /// </summary>
    /// <example>2026-05-16T23:50:00</example>
    public DateTime Date { get; set; }

    /// <summary>
    /// Concepto corto, nota o detalle específico de la transacción.
    /// </summary>
    /// <example>Compra de víveres para la semana</example>
    public string? Description { get; set; }

    /// <summary>
    /// Objeto virtual que representa la relación con la [Categoría] asignada al movimiento.
    /// </summary>
    public virtual Category Category { get; set; } = null!;

    /// <summary>
    /// Objeto virtual que representa la relación con el [Usuario] titular de la transacción.
    /// </summary>
    public virtual User User { get; set; } = null!;
}