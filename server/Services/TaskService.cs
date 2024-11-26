using System.Linq.Expressions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Helpers;
using server.Interfaces;

namespace server.Services;

public class TaskService : ITaskService
{
    private readonly IUnitOfWork _unitOfWork;

    public TaskService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public TaskEntity CreatTask(TaskEntity taskEntity)
    {
        var result = _unitOfWork.Task.Add(taskEntity);
        _unitOfWork.Save();
        return result;
    }

    public TaskEntity GetTask(Guid idUser,long idTask,string? includes)
    {
        var query = _unitOfWork.Task.GetQuery(x => x.Id == idTask, includes)
            .Where(t =>
                t.CreatedBy == idUser ||
                t.TaskUsers.Any(tu => tu.UserId == idUser) ||
                t.TaskDepartments.Any(td => td.Department.DepartmentUsers.Any(du => du.UserId == idUser))
            )
            .Distinct();
        return query.FirstOrDefault();
    }

    public IEnumerable<TaskEntity> GetAllTask()
    {
        return _unitOfWork.Task.Get();
    }

    public TaskUser AssignTaskToUser(TaskUser taskUser)
    {
        var result = _unitOfWork.TaskUser.Add(taskUser);
        _unitOfWork.Save();
        return result;
    }

    public TaskDepartment AssignTaskToDepartment(TaskDepartment taskDepartment)
    {
        var result = _unitOfWork.TaskDepartment.Add(taskDepartment);
        _unitOfWork.Save();
        return result; 

    }

    public TaskEntity DeleteTask(long id)
    {
        var task = _unitOfWork.Task.GetById(id);
        var result = _unitOfWork.Task.Remove(task);
        _unitOfWork.Save();
        return result;
    }

    public TaskEntity UpdateTask(long id, [FromBody] JsonPatchDocument<TaskEntity> patchDoc)
    {
        var task = _unitOfWork.Task.GetById(id);
        // TaskEntity task = null;
        if (task == null) throw new Exception("not found task");

        patchDoc.ApplyTo(task);

        _unitOfWork.Save();

        return task;
    }

    public TaskDepartment UpdateAssignTaskToDepartment(TaskDepartment taskDepartment)
    {
        var result = _unitOfWork.TaskDepartment.Update(taskDepartment);
        _unitOfWork.Save();
        return result;
    }

    public TaskDepartment DeleteAssignTaskToDepartment(long id)
    {
        var taskDepartment = _unitOfWork.TaskDepartment.GetById(id);
        var result = _unitOfWork.TaskDepartment.Remove(taskDepartment);
        _unitOfWork.Save();
        return result;
    }

    public TaskUser UpdateAssignTaskToUser(TaskUser taskUser)
    {
        var result = _unitOfWork.TaskUser.Update(taskUser);
        _unitOfWork.Save();
        return result;
    }

    public TaskUser DeleteAssignTaskToUser(long id)
    {
        var taskUser = _unitOfWork.TaskUser.GetById(id);
        var result = _unitOfWork.TaskUser.Remove(taskUser);
        _unitOfWork.Save();
        return result;
    }

    public IEnumerable<TaskEntity> GetTaskByIdUser(Guid id, Expression<Func<TaskEntity, bool>>? filter,
        string? includeProperties, Pagination? pagination)
    {
        if (Guid.Empty == id) return Enumerable.Empty<TaskEntity>();
        filter ??= t => true;
        var query = _unitOfWork.Task.GetQuery(filter, includeProperties)
            .Where(t =>
                t.CreatedBy == id ||
                t.TaskUsers.Any(tu => tu.UserId == id) ||
                t.TaskDepartments.Any(td => td.Department.DepartmentUsers.Any(du => du.UserId == id))
            )
            .Distinct();
        return query.Paginate(pagination).ToList();
    }

    public IEnumerable<TaskEntity> GetTaskByFilter(Expression<Func<TaskEntity, bool>> filter)
    {
        return _unitOfWork.Task.Get(filter);
    }

    public TaskEntity UpdateTask(TaskEntity taskEntity)
    {
        var result = _unitOfWork.Task.Update(taskEntity);
        _unitOfWork.Save();
        return result;
    }
}