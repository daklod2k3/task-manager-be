using System;
using System.Collections.Generic;

namespace server.Entities;

public partial class ChannelUser
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? UserId { get; set; }

    public long? ChannelId { get; set; }

    public virtual Channel? Channel { get; set; }

    public virtual Profile? User { get; set; }
}
