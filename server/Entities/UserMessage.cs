using System;
using System.Collections.Generic;

namespace server.Entities;

public partial class UserMessage
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid FromId { get; set; }

    public Guid ToId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? UpdatedAt { get; set; }

    public long? FileId { get; set; }

    public virtual Files? File { get; set; }

    public virtual Profile From { get; set; } = null!;

    public virtual Profile To { get; set; } = null!;
}
