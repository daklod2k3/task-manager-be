using System.ComponentModel.DataAnnotations.Schema;

namespace server.Entities;

public class UserMessage
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid? FromId { get; set; }

    public Guid ToId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? UpdatedAt { get; set; }

    public long? FileId { get; set; }

    public virtual FileEntity? File { get; set; }

    public virtual Profile? From { get; set; } = null!;

    public virtual Profile? To { get; set; } = null!;

    [NotMapped] public virtual Profile? SendTo { get; set; } = null!;
}