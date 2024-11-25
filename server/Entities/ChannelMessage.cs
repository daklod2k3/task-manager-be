namespace server.Entities;

public class ChannelMessage
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public long? FileId { get; set; }

    public long? ChannelId { get; set; }

    public virtual Channel? Channel { get; set; }

    public virtual Profile? CreatedByNavigation { get; set; }

    public virtual FileEntity? File { get; set; }
}