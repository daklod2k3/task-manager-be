using System;
using System.Collections.Generic;

namespace server.Entities;

public partial class DepartmentUser
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? UserId { get; set; }

    public long? DepartmentId { get; set; }

    public virtual Department? Department { get; set; }

    public virtual Profile? User { get; set; }
}
