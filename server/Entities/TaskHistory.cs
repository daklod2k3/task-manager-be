namespace server.Entities;

public class TaskHistory
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public long TaskId { get; set; }

    public Guid CreatedBy { get; set; }

    public string Description { get; set; } = null!;
    public ETaskHistoryType Type { get; set; }


    public virtual Profile? User { get; set; } = null!;


    public virtual TaskEntity? TaskEntity { get; set; } = null!;
}