namespace FinTrack.Core.Entities;

public partial class Role : BaseEntity
{
    //public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
