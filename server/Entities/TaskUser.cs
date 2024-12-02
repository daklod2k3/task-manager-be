namespace server.Entities;

public class TaskUser
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid UserId { get; set; }

    public long TaskId { get; set; }

    public virtual TaskEntity? Task { get; set; } = null!;

    public virtual Profile? User { get; set; } = null!;
}