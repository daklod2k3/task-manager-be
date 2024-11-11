using System.Linq.Expressions;
using System.Threading.Tasks;
using LinqKit;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
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
    public TaskEntity GetTask(long id)
    {
        return _unitOfWork.Task.Get(x =>x.Id == id);
    }

    public IEnumerable<TaskEntity> GetAllTask()
    {
        return _unitOfWork.Task.GetAll();
    }

    public int AssignTaskToUser(TaskUser[] taskUsers)
    {
        foreach (var taskUser in taskUsers) _unitOfWork.TaskUser.Add(taskUser);
        return _unitOfWork.Save();
    }

    public int AssignTaskToDepartment(TaskDepartment[] taskDepartments)
    {
        foreach (var taskDepartment in taskDepartments) _unitOfWork.TaskDepartment.Add(taskDepartment);
        return _unitOfWork.Save();
    }

    public TaskEntity DeleteTask(long id)
    {
        var task = _unitOfWork.Task.Get(x => x.Id == id);
        var result = _unitOfWork.Task.Remove(task);
        _unitOfWork.Save();
        return result;
    }

    public TaskEntity UpdateTask(long id, [FromBody] JsonPatchDocument<TaskEntity> patchDoc)
    {
        
        var task =  _unitOfWork.Task.Get(x => x.Id == id);
        if (task == null)
        {
            throw new Exception("not found task");
        }

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
        var taskDepartment = _unitOfWork.TaskDepartment.Get(x => x.Id == id);
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
        var taskUser = _unitOfWork.TaskUser.Get(x => x.Id == id);
        var result = _unitOfWork.TaskUser.Remove(taskUser);
        _unitOfWork.Save();
        return result;
    }

    public IEnumerable<TaskEntity> GetTaskByIdUser(Guid id, Expression<Func<TaskEntity, bool>>? filter)
    {
        if (Guid.Empty == id) return Enumerable.Empty<TaskEntity>();
        var tasksByUser = _unitOfWork.Task.GetAll(filter.And(t => t.TaskUsers.Any(taskUser => taskUser.UserId == id)));
        var tasksByDepartment = _unitOfWork.Task.GetAll(filter.And(
            t => t.TaskDepartments
                .Any(taskDept => taskDept.Department.DepartmentUsers
                    .Any(deptUser => deptUser.UserId == id)))
        );
        var result = tasksByUser.Union(tasksByDepartment).DistinctBy(t => t.Id).ToList();
        return result;
    }

    public IEnumerable<TaskEntity> GetTaskByFilter(Expression<Func<TaskEntity, bool>> filter)
    {
        return _unitOfWork.Task.GetAll(filter);
    }
}