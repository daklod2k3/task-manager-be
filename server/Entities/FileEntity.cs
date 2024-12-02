namespace server.Entities;

public class FileEntity
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid? CreatedBy { get; set; }

    public string Path { get; set; } = null!;

    public virtual Profile? CreatedByNavigation { get; set; }
    public virtual ICollection<ChannelMessage> ChannelMessages { get; set; } = new List<ChannelMessage>();
    public virtual ICollection<UserMessage> UserMessages { get; set; } = new List<UserMessage>();
    public virtual ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
}