using System.ComponentModel.DataAnnotations.Schema;

namespace server.Entities;

public class Department
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string Name { get; set; } = null!;

    public virtual ICollection<DepartmentUser> DepartmentUsers { get; set; } = new List<DepartmentUser>();

    public virtual ICollection<TaskDepartment> TaskDepartments { get; set; } = new List<TaskDepartment>();

    public virtual ICollection<Channel> Channels { get; set; } = new List<Channel>();
    [NotMapped] public double CompleteTask => TaskDepartments?.Count(td => td.Task != null && td.Task.Status == ETaskStatus.Done) ?? 0;
}