namespace server.Entities;

public class Role
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();
}