namespace server.Entities;

public class TaskComment
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public long TaskId { get; set; }

    public Guid CreatedBy { get; set; }

    public string Comment { get; set; } = null!;

    public virtual Profile? User { get; set; }

    public virtual TaskEntity? Task { get; set; }
}