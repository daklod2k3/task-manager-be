using System.ComponentModel.DataAnnotations.Schema;

namespace server.Entities;

public class TaskEntity
{
    public long Id { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public ETaskPriority? Priority { get; set; }

    public ETaskStatus? Status { get; set; } = ETaskStatus.To_do;

    public Guid? CreatedBy { get; set; }
    public long? FileId { get; set; } = null;

    public virtual FileEntity? File { get; set; } = null;
    public virtual ICollection<TaskDepartment> TaskDepartments { get; set; } = new List<TaskDepartment>();

    public virtual ICollection<TaskHistory> TaskHistories { get; set; } = new List<TaskHistory>();

    public virtual ICollection<TaskUser> TaskUsers { get; set; } = new List<TaskUser>();

    public virtual ICollection<TaskComment> TaskComments { get; set; } = new List<TaskComment>();


    [NotMapped] public int AssignedToUser => TaskUsers.Count;

    [NotMapped] public int AssignedToDepartment => TaskDepartments.Count;

    public virtual Profile? CreatedByNavigation { get; set; } = null!;
}