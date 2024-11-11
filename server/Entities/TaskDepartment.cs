namespace server.Entities;

public class TaskDepartment
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public long TaskId { get; set; }

    public long DepartmentId { get; set; }

    public virtual Department? Department { get; set; } = null!;

    public virtual ETask? Task { get; set; } = null!;
}