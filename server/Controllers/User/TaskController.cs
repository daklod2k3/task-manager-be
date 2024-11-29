using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;

namespace server.Controllers.User;

[ApiController]
[Route("/user/[controller]")]
public class TaskController: Controller
{

    private readonly IRepository<Profile> Users;
    private IUnitOfWork _unitOfWork;
    
    public TaskController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        Users = _unitOfWork.User;
    }
    
    [HttpGet]
    public ActionResult GetTasks()
    {
        Guid userId = new Guid(AuthController.GetUserId(HttpContext));
        var taskList = _unitOfWork.Task.Get(t =>
            t.CreatedBy == userId ||
            t.TaskUsers.Any(tu => tu.UserId == userId) ||
            t.TaskDepartments.Any(td => td.Department.DepartmentUsers.Any(du => du.UserId == userId)));
       
        return new SuccessResponse<IEnumerable<TaskEntity>>(taskList);
    }

    [HttpPost]
    public ActionResult CreateTask(TaskEntity taskEntity)
    {
        var id = AuthController.GetUserId(HttpContext);
        taskEntity.CreatedBy = new Guid(id);
        return new SuccessResponse<TaskEntity>(_unitOfWork.Task.Add(taskEntity));
    }

    [HttpPut]
    public ActionResult UpdateTask(TaskEntity taskEntity)
    {
        var id = AuthController.GetUserId(HttpContext);
        var task = GetTask(taskEntity.Id);
        if(task == default){
            return new ErrorResponse("Task is not found");
        }
        if( task.CreatedBy != new Guid(id) && (task.DueDate != taskEntity.DueDate || 
        task.Description != taskEntity.Description  || 
        task.Title != taskEntity.Title || 
        task.Status != taskEntity.Status ||
        task.Priority != taskEntity.Priority ||
        task.CreatedBy != taskEntity.CreatedBy)){
            return new ErrorResponse("You can't change this");
        }
        taskEntity.TaskDepartments = null;
        taskEntity.TaskUsers = null;
        taskEntity.TaskComments = null;
        return new SuccessResponse<TaskEntity>(_unitOfWork.Task.Update(taskEntity));
    }

    public TaskEntity GetTask(long id){
        var iduser = new Guid(AuthController.GetUserId(HttpContext));
        var taskList = _unitOfWork.Task.Get(t =>
            t.CreatedBy == iduser ||
            t.TaskUsers.Any(tu => tu.UserId == iduser) ||
            t.TaskDepartments.Any(td => td.Department.DepartmentUsers.Any(du => du.UserId == iduser)));
        return taskList.FirstOrDefault(t => t.Id == id);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteTask(long id)
    {
        var taskEntity = GetTask(id);
        if(taskEntity == default){
            return new ErrorResponse("Task is not found");
        }
        if(taskEntity.CreatedBy != new Guid(AuthController.GetUserId(HttpContext))){
            return new ErrorResponse("You can't delete this");
        }
        return new SuccessResponse<TaskEntity>(_unitOfWork.Task.Remove(taskEntity));
    }
}