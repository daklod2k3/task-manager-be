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
        if(taskEntity.CreatedBy != new Guid(id))
            return new ErrorResponse("You can't update this task");
        return new SuccessResponse<TaskEntity>(_unitOfWork.Task.Update(taskEntity));
    }

    [HttpPatch("{id}")]
    public ActionResult UpdateTask(long id, [FromBody] JsonPatchDocument<TaskEntity> patchDoc)
    {
        var iduser = AuthController.GetUserId(HttpContext);
        var taskEntity = _unitOfWork.Task.GetById(id);
        if(taskEntity.CreatedBy != new Guid(iduser))
            return new ErrorResponse("You can't update this task");
        return new SuccessResponse<TaskEntity>(_unitOfWork.Task.UpdatePatch(id.ToString(), patchDoc));
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteTask(long id)
    {
        var iduser = AuthController.GetUserId(HttpContext);
        var taskEntity = _unitOfWork.Task.GetById(id);
        if(taskEntity.CreatedBy != new Guid(iduser))
            return new ErrorResponse("You can't update this task");
        return new SuccessResponse<TaskEntity>(_unitOfWork.Task.Remove(taskEntity));
    }
}