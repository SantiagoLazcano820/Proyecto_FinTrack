namespace FinTrack.Core.DTOs
{
    /// <summary>
    /// Representa una [Entidad] de transferencia de datos (DTO) para el balance mensual acumulado.
    /// </summary>
    /// <remarks>
    /// Esta entidad calcula y almacena la información resumida de la salud financiera del usuario 
    /// en un periodo específico, proporcionando totales de ingresos, gastos y listados detallados.
    /// </remarks>
    public class MonthlyBalanceDto
    {
        /// <summary>
        /// Sumatoria total de todos los ingresos percibidos en el mes.
        /// </summary>
        /// <example>5000.00</example>
        public decimal TotalIncomes { get; set; }

        /// <summary>
        /// Sumatoria total de todos los egresos y gastos realizados en el mes.
        /// </summary>
        /// <example>3200.00</example>
        public decimal TotalExpenses { get; set; }

        /// <summary>
        /// Balance neto resultante del cálculo matemático (Ingresos - Egresos).
        /// </summary>
        /// <example>1800.00</example>
        public decimal NetBalance => TotalIncomes - TotalExpenses;

        /// <summary>
        /// Bandera que indica si las finanzas se encuentran bajo un estado de déficit.
        /// </summary>
        /// <example>false</example>
        public bool IsDeficit { get; set; }

        /// <summary>
        /// Estado legible del balance según el resultado económico (Superávit o Déficit).
        /// </summary>
        /// <example>Superávit</example>
        public string Status => NetBalance >= 0 ? "Superávit" : "Déficit";

        /// <summary>
        /// Mensaje analítico o sugerencia devuelta por el sistema sobre el balance calculado.
        /// </summary>
        /// <example>¡Buen trabajo! Tus ingresos superaron tus gastos este mes.</example>
        public string? Message { get; set; }

        /// <summary>
        /// Lista de desglose detallada por categorías correspondiente al balance actual.
        /// </summary>
        public List<CategorySummaryDto> Details { get; set; } = new();
    }
}