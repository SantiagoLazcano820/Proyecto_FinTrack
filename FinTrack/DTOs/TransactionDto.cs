namespace FinTrack.Core.DTOs
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = null!;
        public string Date { get; set; } = null!;
        public string? Description { get; set; }
    }
}
