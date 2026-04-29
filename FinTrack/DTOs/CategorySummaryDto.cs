namespace FinTrack.Core.DTOs
{
    public class CategorySummaryDto
    {
        public string CategoryName { get; set; } = null!;
        public decimal Total { get; set; }
        public string Type { get; set; } = null!;
    }
}
