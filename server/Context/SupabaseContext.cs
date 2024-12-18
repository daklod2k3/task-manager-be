﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using server.Entities;
using server.Services;

namespace server.Context;

public partial class SupabaseContext : DbContext
{
    // private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ICurrentUserService _currentUserService;

    public SupabaseContext()
    {
    }

    public SupabaseContext(DbContextOptions<SupabaseContext> options, ICurrentUserService currentUserService)
        : base(options)
    {
        _currentUserService = currentUserService;
    }

    public virtual DbSet<Channel> Channels { get; set; }

    public virtual DbSet<ChannelMessage> ChannelMessages { get; set; }

    public virtual DbSet<ChannelUser> ChannelUsers { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DepartmentUser> DepartmentUsers { get; set; }

    public virtual DbSet<FileEntity> Files { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<TaskEntity> Tasks { get; set; }

    public virtual DbSet<TaskDepartment> TaskDepartments { get; set; }

    public virtual DbSet<TaskHistory> TaskHistories { get; set; }

    public virtual DbSet<TaskUser> TaskUsers { get; set; }

    public virtual DbSet<UserMessage> UserMessages { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Permission> Permissions { get; set; }
    public virtual DbSet<Resource> Resources { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            // .HasPostgresEnum("TaskPriority", new[] { "High", "Medium", "Low" })
            // .HasPostgresEnum("TaskStatus",
            //     new[] { "To_do", "In_Progress", "In_Preview", "In_Complete", "QA", "Done", "Archived" })
            // .HasPostgresEnum<ETaskPriority>("public", "TaskPriority")
            // .HasPostgresEnum<ETaskStatus>("public", "TaskStatus")
            .HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
            .HasPostgresEnum("auth", "code_challenge_method", new[] { "s256", "plain" })
            .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
            .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn", "phone" })
            .HasPostgresEnum("auth", "one_time_token_type",
                new[]
                {
                    "confirmation_token", "reauthentication_token", "recovery_token", "email_change_token_new",
                    "email_change_token_current", "phone_change_token"
                })
            .HasPostgresEnum("pgsodium", "key_status", new[] { "default", "valid", "invalid", "expired" })
            .HasPostgresEnum("pgsodium", "key_type",
                new[]
                {
                    "aead-ietf", "aead-det", "hmacsha512", "hmacsha256", "auth", "shorthash", "generichash", "kdf",
                    "secretbox", "secretstream", "stream_xchacha20"
                })
            .HasPostgresEnum("realtime", "action", new[] { "INSERT", "UPDATE", "DELETE", "TRUNCATE", "ERROR" })
            .HasPostgresEnum("realtime", "equality_op", new[] { "eq", "neq", "lt", "lte", "gt", "gte", "in" })
            .HasPostgresExtension("extensions", "pg_stat_statements")
            .HasPostgresExtension("extensions", "pgcrypto")
            .HasPostgresExtension("extensions", "pgjwt")
            .HasPostgresExtension("extensions", "uuid-ossp")
            .HasPostgresExtension("graphql", "pg_graphql")
            .HasPostgresExtension("pgsodium", "pgsodium")
            .HasPostgresExtension("vault", "supabase_vault");

        modelBuilder.Entity<Channel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("channels_pkey");

            entity.ToTable("channels");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");

            entity.HasOne(d => d.Department).WithMany(p => p.Channels)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("channels_department_id_fkey");
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Channels)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("channels_created_by_fkey");
        });

        modelBuilder.Entity<ChannelMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("channel_message_pkey");

            entity.ToTable("channel_message");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChannelId).HasColumnName("channel_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.FileId).HasColumnName("file_id");
            entity.Property(e => e.Content).HasColumnName("content");

            entity.HasOne(d => d.Channel).WithMany(p => p.ChannelMessages)
                .HasForeignKey(d => d.ChannelId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("channel_message_channel_id_fkey");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ChannelMessages)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("channel_message_created_by_fkey");

            entity.HasOne(d => d.File).WithMany(p => p.ChannelMessages)
                .HasForeignKey(d => d.FileId)
                .HasConstraintName("channel_message_file_id_fkey");
        });

        modelBuilder.Entity<ChannelUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("channel_user_pkey");

            entity.ToTable("channel_user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChannelId).HasColumnName("channel_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Channel).WithMany(p => p.ChannelUsers)
                .HasForeignKey(d => d.ChannelId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("channel_user_channel_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.ChannelUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("channel_user_user_id_fkey");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("departments_pkey");

            entity.ToTable("departments");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<DepartmentUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("department_user_pkey");

            entity.ToTable("department_user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.OwnerType).HasColumnName("owner_type");

            entity.HasOne(d => d.Department).WithMany(p => p.DepartmentUsers)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("department_user_department_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.DepartmentUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("department_user_user_id_fkey");
        });

        modelBuilder.Entity<FileEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("files_pkey");

            entity.ToTable("files");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Path)
                .HasColumnType("character varying")
                .HasColumnName("path");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Files)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("files_created_by_fkey");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("notifications_pkey");

            entity.ToTable("notifications");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Read)
                .HasDefaultValue(false)
                .HasColumnName("read");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("notifications_user_id_fkey");
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("resources_pkey");

            entity.ToTable("resources");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Path)
                .HasColumnType("character varying")
                .HasColumnName("path");
        });


        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("permissions_pkey");

            entity.ToTable("permissions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Create).HasColumnName("create");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Delete).HasColumnName("delete");
            entity.Property(e => e.ResourceId).HasColumnName("resource_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Update).HasColumnName("update");
            entity.Property(e => e.View).HasColumnName("view");

            entity.HasOne(d => d.Resource).WithMany(p => p.Permissions)
                .HasForeignKey(d => d.ResourceId)
                .HasConstraintName("permissions_resource_id_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Permissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("permissions_role_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("profiles_pkey");

            entity.ToTable("profiles");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Avt).HasColumnName("avt");
            entity.Property(e => e.Bio).HasColumnName("bio");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.HasOne(d => d.Role).WithMany(p => p.Profiles)
                .HasForeignKey(d => d.RoleId).HasConstraintName("profiles_role_id_fkey");
        });

        modelBuilder.Entity<TaskEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tasks_pkey");

            entity.ToTable("tasks");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DueDate).HasColumnName("due_date");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Priority).HasColumnName("priority");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.FileId).HasColumnName("file_id");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("tasks_created_by_fkey");
            entity.HasOne(e => e.File).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.FileId)
                .HasConstraintName("tasks_file_id_fkey");
        });

        modelBuilder.Entity<TaskDepartment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("task_department_pkey");

            entity.ToTable("task_department");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.TaskId).HasColumnName("task_id");

            entity.HasOne(d => d.Department).WithMany(p => p.TaskDepartments)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("task_department_department_id_fkey");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskDepartments)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("task_department_task_id_fkey");
        });

        modelBuilder.Entity<TaskHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("task_history_pkey");

            entity.ToTable("task_history");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.User).WithMany(p => p.TaskHistories)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("task_history_created_by_fkey");

            entity.HasOne(d => d.TaskEntity).WithMany(p => p.TaskHistories)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("task_history_task_id_fkey");
        });

        modelBuilder.Entity<TaskUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("task_user_pkey");

            entity.ToTable("task_user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskUsers)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("task_user_task_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.TaskUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("task_user_user_id_fkey");
        });

        modelBuilder.Entity<TaskComment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("task_comment_pkey");

            entity.ToTable("task_comment");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Comment)
                .HasColumnType("character varying")
                .HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.TaskId).HasColumnName("task_id");

            entity.HasOne(d => d.User).WithMany(p => p.TaskComments)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("task_comment_created_by_fkey");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskComments)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("task_comment_task_id_fkey");
        });

        modelBuilder.Entity<UserMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_message_pkey");

            entity.ToTable("user_message");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.FileId).HasColumnName("file_id");
            entity.Property(e => e.FromId).HasColumnName("from_id");
            entity.Property(e => e.ToId).HasColumnName("to_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.File).WithMany(p => p.UserMessages)
                .HasForeignKey(d => d.FileId)
                .HasConstraintName("user_message_file_id_fkey");

            entity.HasOne(d => d.From).WithMany(p => p.UserMessageFroms)
                .HasForeignKey(d => d.FromId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_message_from_id_fkey");

            entity.HasOne(d => d.To).WithMany(p => p.UserMessageTos)
                .HasForeignKey(d => d.ToId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_message_to_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public override int SaveChanges()
    {
        DetectHistoryChange();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new())
    {
        DetectHistoryChange();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }


    private async Task<string> getUserIdAuth()
    {
        try
        {
            return _currentUserService.UserId;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine("dbcontext can't get auth user: " + e);
        }

        return null;
    }

    private async void DetectHistoryChange()
    {
        var taskEntries = ChangeTracker.Entries<TaskEntity>();
        foreach (var entry in taskEntries) CreateTaskHistoryFromTaskUpdate(entry);
        var taskUsersEntries = ChangeTracker.Entries<TaskUser>();
        foreach (var entry in taskUsersEntries) CreateTaskHistoryUserAssign(entry);
        var taskDepartmentsEntries = ChangeTracker.Entries<TaskDepartment>();
        foreach (var entry in taskDepartmentsEntries) CreateTaskHistoryDepartmentAssign(entry);
    }

    private async void CreateTaskHistoryFromTaskUpdate(EntityEntry<TaskEntity> entry)
    {
        // Debug.WriteLine(entry.Property("Priority").CurrentValue);
        // Debug.WriteLine(entry.Property("Priority").OriginalValue);
        var history = new List<TaskHistory>();
        var user_id = await getUserIdAuth();
        if (entry.State != EntityState.Modified) return;
        if (!entry.Property("Status").OriginalValue?.Equals(entry.Property("Status").CurrentValue) ??
            !entry.Property("Status").CurrentValue?.Equals(entry.Property("Status").OriginalValue) ?? false)
            history.Add(new TaskHistory
            {
                TaskId = entry.Entity.Id,
                CreatedBy = new Guid(user_id),
                Description = "changed task **Status** from **" + entry.Property("Status").OriginalValue + "** to **" +
                              entry.Property("Status").CurrentValue + "**"
            });
        if (entry.OriginalValues.GetValue<ETaskPriority?>("Priority") !=
            entry.CurrentValues.GetValue<ETaskPriority?>("Priority"))
            history.Add(new TaskHistory
            {
                TaskId = entry.Entity.Id,
                CreatedBy = new Guid(user_id),
                Description = "changed task **Priority** from **" + entry.Property("Priority").OriginalValue +
                              "** to **" +
                              entry.Property("Priority").CurrentValue + "**"
            });
        if (!entry.Property("DueDate").OriginalValue?.Equals(entry.Property("DueDate").CurrentValue) ??
            !entry.Property("DueDate").CurrentValue?.Equals(entry.Property("DueDate").OriginalValue) ?? false)
            history.Add(new TaskHistory
            {
                TaskId = entry.Entity.Id,
                CreatedBy = new Guid(user_id),
                Description = "set task **Due Date** from **" + (entry.Property("DueDate").OriginalValue ?? "none") +
                              "** to **" +
                              entry.Property("DueDate").CurrentValue + "**"
            });
        if (!entry.Property("Title").OriginalValue?.Equals(entry.Property("Title").CurrentValue) ??
            !entry.Property("Title").CurrentValue?.Equals(entry.Property("Title").OriginalValue) ?? false)
            history.Add(new TaskHistory
            {
                TaskId = entry.Entity.Id,
                CreatedBy = new Guid(user_id),
                Description = "change task name from **" + entry.Property("Title").OriginalValue + "** to **" +
                              entry.Property("Title").CurrentValue + "**"
            });
        TaskHistories.AddRange(history);
    }

    private void CreateTaskHistoryUserAssign(EntityEntry<TaskUser> entry)
    {
    }

    private void CreateTaskHistoryDepartmentAssign(EntityEntry<TaskDepartment> entry)
    {
    }
}