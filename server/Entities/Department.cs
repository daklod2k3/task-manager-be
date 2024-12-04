namespace server.Entities;

public class Department
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string Name { get; set; } = null!;

    public virtual ICollection<DepartmentUser> DepartmentUsers { get; set; } = new List<DepartmentUser>();

    public virtual ICollection<TaskDepartment> TaskDepartments { get; set; } = new List<TaskDepartment>();
}