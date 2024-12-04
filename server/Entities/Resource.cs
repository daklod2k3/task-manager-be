namespace server.Entities;

public class Resource
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? Name { get; set; }

    public string Path { get; set; } = null!;

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}