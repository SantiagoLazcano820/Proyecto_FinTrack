namespace FinTrack.Core.DTOs
{
    public class MonthlyBalanceDto
    {
        public decimal TotalIncomes { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetBalance => TotalIncomes - TotalExpenses;
        public bool IsDeficit { get; set; }
        public string Status => NetBalance >= 0 ? "Superávit" : "Déficit";
        public string? Message { get; set; }
        public List<CategorySummaryDto> Details { get; set; } = new();
    }
}
