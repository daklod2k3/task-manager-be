namespace server.Entities;

public class TaskHistory
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public long TaskId { get; set; }

    public Guid CreatedBy { get; set; }

    public string Description { get; set; } = null!;

    public virtual Profile? CreatedByNavigation { get; set; } = null!;

    public virtual TaskEntity? TaskEntity { get; set; } = null!;
}