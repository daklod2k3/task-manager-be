using System.ComponentModel.DataAnnotations.Schema;

namespace server.Entities;

public class TaskEntity
{
    public long Id { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public ETaskPriority? Priority { get; set; }

    public ETaskStatus? Status { get; set; } = ETaskStatus.To_do;

    public Guid? CreatedBy { get; set; }

    public virtual ICollection<TaskDepartment> TaskDepartments { get; set; } = new List<TaskDepartment>();

    public virtual ICollection<TaskHistory> TaskHistories { get; set; } = new List<TaskHistory>();

    public virtual ICollection<TaskUser> TaskUsers { get; set; } = new List<TaskUser>();

    [NotMapped]
    public int AssignedToUser
    {
        get
        {
            return TaskUsers.Count;
        }
    }

    [NotMapped]
    public int AssignedToDepartment
    {
        get
        {
            return TaskDepartments.Count;
        }
    }
    public virtual Profile? CreatedByNavigation { get; set; } = null!;
}