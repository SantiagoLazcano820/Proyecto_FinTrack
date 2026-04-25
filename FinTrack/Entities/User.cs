namespace FinTrack.Core.Entities;

public partial class User : BaseEntity
{
    //public int Id { get; set; }

    public int RoleId { get; set; }

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public ulong IsActive { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
