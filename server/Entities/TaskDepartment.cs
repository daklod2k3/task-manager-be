﻿namespace server.Entities;

public class TaskDepartment
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public long TaskId { get; set; }

    public long DepartmentId { get; set; }

    public virtual Department? Department { get; set; } = null!;

    public virtual TaskEntity? Task { get; set; } = null!;
}