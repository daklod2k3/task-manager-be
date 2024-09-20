using System;
using System.Collections.Generic;

namespace server.Entities;

public partial class Tasks
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime DueDate { get; set; }
    
    public String Priority { get; set; }

    public String Status { get; set; }
    
    public virtual ICollection<TaskDepartment> TaskDepartments { get; set; } = new List<TaskDepartment>();

    public virtual ICollection<TaskHistory> TaskHistories { get; set; } = new List<TaskHistory>();

    public virtual ICollection<TaskUser> TaskUsers { get; set; } = new List<TaskUser>();
}
