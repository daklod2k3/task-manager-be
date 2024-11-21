﻿using System;
using System.Collections.Generic;

namespace server.Entities;

public partial class TaskComment
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public long? TaskId { get; set; }

    public Guid? CreatedBy { get; set; }

    public string Comment { get; set; } = null!;

    public virtual Profile? CreatedByNavigation { get; set; }

    public virtual TaskEntity? Task { get; set; }
}