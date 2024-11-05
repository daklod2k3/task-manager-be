using LinqKit;
using server.Entities;
using server.Interfaces;
using System.Linq.Expressions;

namespace server.Services;

public class TaskService : ITaskService
{
    private readonly IUnitOfWork _unitOfWork;

    public TaskService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public ETask CreatTask(ETask eTask)
    {
        var result = _unitOfWork.Task.Add(eTask);
        _unitOfWork.Save();
        return result;
    }

    public IEnumerable<ETask> GetAllTask()
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

    public ETask DeleteTask(long id)
    {
        var task = _unitOfWork.Task.Get(x => x.Id == id);
        var result = _unitOfWork.Task.Remove(task);
        _unitOfWork.Save();
        return result;
    }

    public ETask UpdateTask(ETask eTask)
    {
        var result = _unitOfWork.Task.Update(eTask);
        _unitOfWork.Save();
        return result;
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

    public IEnumerable<ETask> GetTaskByIdUser(Guid id, Expression<Func<ETask, bool>>? filter)
    {
        if (Guid.Empty == id)
        {
            return Enumerable.Empty<ETask>();
        }
        var tasksByUser = _unitOfWork.Task.GetAll(filter: filter.And(t => t.TaskUsers.Any(taskUser => taskUser.UserId == id)));
        var tasksByDepartment = _unitOfWork.Task.GetAll(filter: filter.And(
            t => t.TaskDepartments
                .Any(taskDept => taskDept.Department.DepartmentUsers
                    .Any(deptUser => deptUser.UserId == id)))
        );
        var result = tasksByUser.Union(tasksByDepartment).DistinctBy(t => t.Id).ToList();
        return result;
    }
    public IEnumerable<ETask> GetTaskByFilter(Expression<Func<ETask, bool>> filter)
    {
        return _unitOfWork.Task.GetAll(filter: filter);
    }
}