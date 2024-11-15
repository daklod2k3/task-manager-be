using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using System.Linq.Expressions;

namespace server.Interfaces;

public interface ITaskService
{
    IEnumerable<TaskEntity> GetAllTask();
    TaskEntity CreatTask(TaskEntity taskEntity);
    public TaskEntity GetTask(long id);
    int AssignTaskToDepartment(TaskDepartment[] taskDepartments);
    int AssignTaskToUser(TaskUser[] taskUsers);
    TaskEntity DeleteTask(long idTask);
    TaskEntity UpdateTask(long id, [FromBody] JsonPatchDocument<TaskEntity> patchDoc);
    TaskDepartment UpdateAssignTaskToDepartment(TaskDepartment taskDepartment);
    TaskDepartment DeleteAssignTaskToDepartment(long id);
    TaskUser UpdateAssignTaskToUser(TaskUser taskUser);
    TaskUser DeleteAssignTaskToUser(long id);
    public IEnumerable<TaskEntity> GetTaskByIdUser(Guid id, Expression<Func<TaskEntity, bool>>? compositeFilterExpression, string includes = "");
    public IEnumerable<TaskEntity> GetTaskByFilter(Expression<Func<TaskEntity, bool>> compositeFilterExpression);
}