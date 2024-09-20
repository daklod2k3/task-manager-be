using System;
using System.Collections.Generic;

namespace server.Entities;

public partial class TaskUser
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid UserId { get; set; }

    public long TaskId { get; set; }

    public virtual Tasks Tasks { get; set; } = null!;

    public virtual Profile User { get; set; } = null!;
}
