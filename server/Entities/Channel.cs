namespace server.Entities;

public class Channel
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string Name { get; set; } = null!;

    public Guid? CreatedBy { get; set; }

    public long? DepartmentId { get; set; } = null!;

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<ChannelMessage> ChannelMessages { get; set; } = new List<ChannelMessage>();

    public virtual ICollection<ChannelUser> ChannelUsers { get; set; } = new List<ChannelUser>();

    public virtual Profile? CreatedByNavigation { get; set; } = null!;
}