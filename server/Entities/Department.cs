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

    [NotMapped]
    public string DepartmentOwner
    {
        get
        {
            if (DepartmentUsers == null) return "no owner";

            foreach (var du in DepartmentUsers)
            {
                if (du.OwnerType == EDepartmentOwnerType.Owner && du.User != null)
                {
                    return du.User.Name;
                }
            }

            return "no owner";
        }
    }
    [NotMapped]
    public double CompleteTask => (TaskDepartments != null && TaskDepartments.Any())
        ? (TaskDepartments.Count(td => td.Task != null && td.Task.Status == ETaskStatus.Done) / (double)TaskDepartments.Count()) * 100
        : 0;
}