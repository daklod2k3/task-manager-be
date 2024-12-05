namespace server.Entities;

public class Notification
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid UserId { get; set; }

    public bool? Read { get; set; }

    public string Content { get; set; } = null!;

    public virtual Profile? User { get; set; } = null!;
}