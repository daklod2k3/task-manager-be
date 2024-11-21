using System;
using System.Collections.Generic;

namespace server.Entities;

public partial class Resource
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Name { get; set; }

    public string Path { get; set; } = null!;

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
