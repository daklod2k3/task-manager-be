using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;

namespace server.Controllers.User;

[ApiController]
[Route("/user/[controller]")]
public class TaskHistoryController: Controller
{

    private readonly IRepository<Profile> Users;
    private readonly IRepository<TaskHistory> _repository;
    
    public TaskHistoryController(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.TaskHistories;
        Users = unitOfWork.Users;
    }
    
    [HttpGet]
    public ActionResult GetTaskHistories()
    {
        Guid userId = new Guid(AuthController.GetUserId(HttpContext));
        var taskhistoryList = _repository.Get(t =>
            t.CreatedBy == userId);
       
        return new SuccessResponse<IEnumerable<TaskHistory>>(taskhistoryList);
    }

    [HttpPost]
    public ActionResult CreateTaskHistory(TaskHistory taskHistory)
    {
        var id = AuthController.GetUserId(HttpContext);
        taskHistory.CreatedBy = new Guid(id);
        var result = _repository.Add(taskHistory);
        _repository.Save();
        return new SuccessResponse<TaskHistory>(result);
    }

    [HttpPut]
    public ActionResult UpdateTaskHistory(TaskHistory taskHistory)
    {
        var id = AuthController.GetUserId(HttpContext);
        var taskhistory = GetTaskHistory(taskHistory.Id);
        if(taskhistory == default){
            return new ErrorResponse("TaskHistory is not found");
        }
        if(taskhistory.CreatedBy != new Guid(id)){
            return new ErrorResponse("You can't change this");
        }
        if( taskhistory.CreatedBy == new Guid(id) 
        && (taskhistory.TaskId != taskHistory.TaskId || 
        taskhistory.CreatedAt != taskHistory.CreatedAt ||
        taskhistory.CreatedBy != taskHistory.CreatedBy)){
            return new ErrorResponse("You can't change this");
        }
        taskHistory.User = null;
        taskHistory.TaskEntity = null;
        var result = _repository.Update(taskHistory);
        _repository.Save();
        return new SuccessResponse<TaskHistory>(result);
    }

    public TaskHistory GetTaskHistory(long id){
        var iduser = new Guid(AuthController.GetUserId(HttpContext));
        var taskhistoryList = _repository.Get(t =>
            t.CreatedBy == iduser);
        return taskhistoryList.FirstOrDefault(t => t.Id == id);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteTaskHistory(long id)
    {
        var taskHistory = GetTaskHistory(id);
        if(taskHistory == default){
            return new ErrorResponse("TaskHistory is not found");
        }
        if(taskHistory.CreatedBy != new Guid(AuthController.GetUserId(HttpContext))){
            return new ErrorResponse("You can't delete this");
        }
        var result = _repository.Remove(taskHistory);
        _repository.Save();
        return new SuccessResponse<TaskHistory>(result);
    }
}