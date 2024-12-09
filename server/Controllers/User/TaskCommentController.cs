using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;

namespace server.Controllers.User;

[ApiController]
[Route("/user/[controller]")]
public class TaskCommentController: Controller
{

    private readonly IRepository<Profile> Users;
    private readonly IRepository<TaskComment> _repository;
    
    public TaskCommentController(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.TaskComments;
        Users = unitOfWork.Users;
    }
    
    [HttpGet]
    public ActionResult GetTaskComments()
    {
        Guid userId = new Guid(AuthController.GetUserId(HttpContext));
        var taskcommentList = _repository.Get(t =>
            t.CreatedBy == userId ||
            t.Task.TaskComments.Any(tc => tc.CreatedBy == userId));
       
        return new SuccessResponse<IEnumerable<TaskComment>>(taskcommentList);
    }

    [HttpPost]
    public ActionResult CreateTaskComment(TaskComment taskcomment)
    {
        var id = AuthController.GetUserId(HttpContext);
        taskcomment.CreatedBy = new Guid(id);
        var result = _repository.Add(taskcomment);
        _repository.Save();
        return new SuccessResponse<TaskComment>(result);
    }

    [HttpPut]
    public ActionResult UpdateTaskComment(TaskComment TaskComment)
    {
        var id = AuthController.GetUserId(HttpContext);
        var taskcomment = GetTaskComment(TaskComment.Id);
        if(taskcomment == default){
            return new ErrorResponse("TaskComment is not found");
        }
        if(taskcomment.CreatedBy != new Guid(id)){
            return new ErrorResponse("You can't change this");
        }
        if( taskcomment.CreatedBy == new Guid(id) && 
        (taskcomment.CreatedAt != TaskComment.CreatedAt || 
        taskcomment.TaskId != TaskComment.TaskId)){
            return new ErrorResponse("You can't change this");
        }
        taskcomment.User = null;
        taskcomment.Task = null;
        var result = _repository.Update(taskcomment);
        _repository.Save();
        return new SuccessResponse<TaskComment>(result);
    }

    public TaskComment GetTaskComment(long id){
        var iduser = new Guid(AuthController.GetUserId(HttpContext));
        var taskcommentList = _repository.Get(t =>
            t.CreatedBy == iduser);
        return taskcommentList.FirstOrDefault(t => t.Id == id);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteTaskComment(long id)
    {
        var taskcomment = GetTaskComment(id);
        if(taskcomment == default){
            return new ErrorResponse("TaskComment is not found");
        }
        if(taskcomment.CreatedBy != new Guid(AuthController.GetUserId(HttpContext))){
            return new ErrorResponse("You can't delete this");
        }
        var result = _repository.Remove(taskcomment);
        _repository.Save();
        return new SuccessResponse<TaskComment>(result);
    }
}