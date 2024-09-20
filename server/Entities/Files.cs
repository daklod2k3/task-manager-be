using System;
using System.Collections.Generic;

namespace server.Entities;

public partial class Files
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public string Path { get; set; } = null!;

    public virtual ICollection<ChannelMessage> ChannelMessages { get; set; } = new List<ChannelMessage>();

    public virtual Profile? CreatedByNavigation { get; set; }

    public virtual ICollection<UserMessage> UserMessages { get; set; } = new List<UserMessage>();
}
