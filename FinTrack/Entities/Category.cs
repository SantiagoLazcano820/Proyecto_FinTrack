namespace FinTrack.Core.Entities;

public partial class Category : BaseEntity
{
    //public int Id { get; set; }
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public ulong IsActive { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
