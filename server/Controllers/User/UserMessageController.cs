using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;

namespace server.Controllers.User;

[ApiController]
[Route("/user/[controller]")]
public class UserMessageController: Controller
{

    private readonly IRepository<Profile> Users;
    private readonly IRepository<UserMessage> _repository;
    
    public UserMessageController(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.UserMessages;
        Users = unitOfWork.Users;
    }
    
    [HttpGet]
    public ActionResult GetUserMessages()
    {
        Guid userId = new Guid(AuthController.GetUserId(HttpContext));
        var usermessageList = _repository.Get(t =>
            t.FromId == userId ||
            t.ToId == userId);
       
        return new SuccessResponse<IEnumerable<UserMessage>>(usermessageList);
    }

    [HttpPost]
    public ActionResult CreateUserMessage(UserMessage userMessage)
    {
        var id = AuthController.GetUserId(HttpContext);
        userMessage.FromId = new Guid(id);
        var result = _repository.Add(userMessage);
        _repository.Save();
        return new SuccessResponse<UserMessage>(result);
    }

    [HttpPut]
    public ActionResult UpdateUserMessage(UserMessage userMessage)
    {
        var id = AuthController.GetUserId(HttpContext);
        var usermessage = GetUserMessage(userMessage.Id);
        if(usermessage == default){
            return new ErrorResponse("UserMessage is not found");
        }
        if(usermessage.FromId != new Guid(id)){
            return new ErrorResponse("You can't change this");
        }
        if( usermessage.FromId == new Guid(id) && 
        (usermessage.Id != userMessage.Id || 
        usermessage.ToId != userMessage.ToId  || 
        usermessage.FileId != userMessage.FileId || 
        usermessage.CreatedAt != userMessage.CreatedAt)){
            return new ErrorResponse("You can't change this");
        }
        userMessage.File = null;
        userMessage.From = null;
        userMessage.To = null;
        userMessage.UpdatedAt = DateTime.Now;
        var result = _repository.Update(userMessage);
        _repository.Save();
        return new SuccessResponse<UserMessage>(result);
    }

    public UserMessage GetUserMessage(long id){
        var iduser = new Guid(AuthController.GetUserId(HttpContext));
        var usermessageList = _repository.Get(t =>
            t.FromId == iduser ||
            t.ToId == iduser);
        return usermessageList.FirstOrDefault(t => t.Id == id);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteUserMessage(long id)
    {
        var userMessage = GetUserMessage(id);
        if(userMessage == default){
            return new ErrorResponse("UserMessage is not found");
        }
        if(userMessage.FromId != new Guid(AuthController.GetUserId(HttpContext))){
            return new ErrorResponse("You can't delete this");
        }
        var result = _repository.Remove(userMessage);
        _repository.Save();
        return new SuccessResponse<UserMessage>(result);
    }
}