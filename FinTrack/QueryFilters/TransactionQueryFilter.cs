namespace FinTrack.Core.QueryFilters
{
    public class TransactionQueryFilter
    {
        public int? UserId { get; set; }
        public int? CategoryId { get; set; }
        public string? Type { get; set; }
        public string? Date { get; set; }
        public string? Description { get; set; }
    }
}
