namespace server.Entities;

public class ChannelUser
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid? UserId { get; set; }

    public long? ChannelId { get; set; }

    public virtual Channel? Channel { get; set; }

    public virtual Profile? User { get; set; }
}