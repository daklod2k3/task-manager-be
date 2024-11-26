using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace server.Entities;

public class Profile
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Bio { get; set; }

    public string? Avt { get; set; }

    public long RoleId { get; set; }


    public virtual Role Role { get; set; } = null!;

    [JsonIgnore] public virtual ICollection<ChannelMessage> ChannelMessages { get; set; } = new List<ChannelMessage>();

    [JsonIgnore] public virtual ICollection<ChannelUser> ChannelUsers { get; set; } = new List<ChannelUser>();

    [JsonIgnore] public virtual ICollection<Channel> Channels { get; set; } = new List<Channel>();

    [JsonIgnore] public virtual ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();

    [JsonIgnore] public virtual ICollection<TaskComment> TaskComments { get; set; } = new List<TaskComment>();

    public virtual ICollection<DepartmentUser> DepartmentUsers { get; set; } = new List<DepartmentUser>();

    [JsonIgnore] public virtual ICollection<FileEntity> Files { get; set; } = new List<FileEntity>();

    [JsonIgnore] public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    [JsonIgnore] public virtual ICollection<TaskHistory> TaskHistories { get; set; } = new List<TaskHistory>();

    [JsonIgnore] public virtual ICollection<TaskUser> TaskUsers { get; set; } = new List<TaskUser>();

    [JsonIgnore] public virtual ICollection<UserMessage> UserMessageFroms { get; set; } = new List<UserMessage>();

    [JsonIgnore] public virtual ICollection<UserMessage> UserMessageTos { get; set; } = new List<UserMessage>();

    [NotMapped] public string RoleName => Role?.Name;
}