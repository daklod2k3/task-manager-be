﻿using server.Entities;
using System.Linq.Expressions;

namespace server.Interfaces;

public interface ITaskService
{
    IEnumerable<ETask> GetAllTask();
    ETask CreatTask(ETask eTask);
    int AssignTaskToDepartment(TaskDepartment[] taskDepartments);
    int AssignTaskToUser(TaskUser[] taskUsers);
    ETask DeleteTask(long idTask);
    ETask UpdateTask(ETask eTask);
    TaskDepartment UpdateAssignTaskToDepartment(TaskDepartment taskDepartment);
    TaskDepartment DeleteAssignTaskToDepartment(long id);
    TaskUser UpdateAssignTaskToUser(TaskUser taskUser);
    TaskUser DeleteAssignTaskToUser(long id);
    public IEnumerable<ETask> GetTaskByIdUser(Guid id, Expression<Func<ETask, bool>>? compositeFilterExpression);
    public IEnumerable<ETask> GetTaskByFilter(Expression<Func<ETask, bool>> compositeFilterExpression);
}