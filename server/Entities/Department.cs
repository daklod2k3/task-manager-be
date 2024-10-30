using System;
using System.Collections.Generic;

namespace server.Entities;

public partial class Department
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<DepartmentUser> DepartmentUsers { get; set; } = new List<DepartmentUser>();

    public virtual ICollection<TaskDepartment> TaskDepartments { get; set; } = new List<TaskDepartment>();
}
