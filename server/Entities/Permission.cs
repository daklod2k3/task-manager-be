using System;
using System.Collections.Generic;

namespace server.Entities;

public partial class Permission
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool View { get; set; }

    public bool Create { get; set; }

    public bool Update { get; set; }

    public bool Delete { get; set; }

    public long ResourceId { get; set; }

    public long? RoleId { get; set; }

    public virtual Resource Resource { get; set; } = null!;

    public virtual Role? Role { get; set; }
}
