using System;
using System.Collections.Generic;

namespace server.Entities;

public partial class Channel
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Name { get; set; } = null!;

    public Guid CreatedBy { get; set; }

    public virtual ICollection<ChannelMessage> ChannelMessages { get; set; } = new List<ChannelMessage>();

    public virtual ICollection<ChannelUser> ChannelUsers { get; set; } = new List<ChannelUser>();

    public virtual Profile CreatedByNavigation { get; set; } = null!;
}
