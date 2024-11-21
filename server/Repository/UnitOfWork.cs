﻿using server.Context;
using server.Interfaces;

namespace server.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly SupabaseContext _context;

    public UnitOfWork(SupabaseContext context)
    {
        _context = context;
        Task = new TaskRepository(_context);
        TaskDepartment = new TaskDepartmentRepository(_context);
        TaskUser = new TaskUserRepository(_context);
        User = new UserRepository(_context);
        TaskHistory = new TaskHistoryRepository(_context);
        Department = new DepartmentRepository(_context);
        DepartmentUser = new DepartmentUserRepository(_context);
    }

    public ITaskRepository Task { get; }
    public ITaskDepartmentRepository TaskDepartment { get; }
    public ITaskUserRepository TaskUser { get; }
    public IUserRepository User { get; }
    public ITaskHistoryRepository TaskHistory { get; }
    public IDepartmentRepository Department { get; }
    public IDepartmentUserRepository DepartmentUser { get; }
    public int Save()
    {
        return _context.SaveChanges();
    }
}