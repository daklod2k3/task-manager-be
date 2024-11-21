using System;
using System.Collections.Generic;

namespace server.Entities;

public partial class Role
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
