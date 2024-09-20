using System;
using System.Collections.Generic;

namespace server.Entities;

public partial class Notification
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid UserId { get; set; }

    public bool? Read { get; set; }

    public string Content { get; set; } = null!;

    public virtual Profile User { get; set; } = null!;
}
