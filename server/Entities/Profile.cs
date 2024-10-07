namespace server.Entities;

public class Profile
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Bio { get; set; }

    public string? Avt { get; set; }

    public virtual ICollection<ChannelMessage> ChannelMessages { get; set; } = new List<ChannelMessage>();

    public virtual ICollection<ChannelUser> ChannelUsers { get; set; } = new List<ChannelUser>();

    public virtual ICollection<Channel> Channels { get; set; } = new List<Channel>();

    public virtual ICollection<DepartmentUser> DepartmentUsers { get; set; } = new List<DepartmentUser>();

    public virtual ICollection<Files> Files { get; set; } = new List<Files>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<TaskHistory> TaskHistories { get; set; } = new List<TaskHistory>();

    public virtual ICollection<TaskUser> TaskUsers { get; set; } = new List<TaskUser>();

    public virtual ICollection<UserMessage> UserMessageFroms { get; set; } = new List<UserMessage>();

    public virtual ICollection<UserMessage> UserMessageTos { get; set; } = new List<UserMessage>();
}