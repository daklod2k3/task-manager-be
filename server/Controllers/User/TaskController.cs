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
}