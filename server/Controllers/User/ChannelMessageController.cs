using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;

namespace server.Controllers.User;

[ApiController]
[Route("/user/[controller]")]
public class ChannelMessageController: Controller
{

    private readonly IRepository<Profile> Users;
    private readonly IRepository<ChannelMessage> _repository;
    
    public ChannelMessageController(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.ChannelMessages;
        Users = unitOfWork.Users;
    }
    
    [HttpGet]
    public ActionResult GetChannelMessages()
    {
        Guid userId = new Guid(AuthController.GetUserId(HttpContext));
        var channelList = _repository.Get(t =>
            t.CreatedBy == userId ||
            t.Channel.ChannelUsers.Any(tu => tu.UserId == userId));
            
        return new SuccessResponse<IEnumerable<ChannelMessage>>(channelList);
    }

    [HttpPost]
    public ActionResult CreateChannelMessage(ChannelMessage channelMessage)
    {
        var id = AuthController.GetUserId(HttpContext);
        channelMessage.CreatedBy = new Guid(id);
        var result = _repository.Add(channelMessage);
        _repository.Save();
        return new SuccessResponse<ChannelMessage>(result);
    }

    [HttpPut]
    public ActionResult UpdateChannelMessage(ChannelMessage ChannelMessage)
    {
        var id = AuthController.GetUserId(HttpContext);
        var channelmessage = GetChannelMessage(ChannelMessage.Id);
        if(channelmessage == default){
            return new ErrorResponse("ChannelMessage is not found");
        }
        if(channelmessage.CreatedBy != new Guid(id)){
            return new ErrorResponse("You can't change this");
        }
        if( channelmessage.CreatedBy == new Guid(id) && (
        channelmessage.FileId != ChannelMessage.FileId || 
        channelmessage.CreatedBy != ChannelMessage.CreatedBy  || 
        channelmessage.ChannelId != ChannelMessage.ChannelId || 
        channelmessage.Channel != ChannelMessage.Channel)){
            return new ErrorResponse("You can't change this");
        }
        channelmessage.Channel = null;
        channelmessage.File = null;
        var result = _repository.Update(channelmessage);
        _repository.Save();
        return new SuccessResponse<ChannelMessage>(result);
    }

    public ChannelMessage GetChannelMessage(long id){
        var iduser = new Guid(AuthController.GetUserId(HttpContext));
        var channelmessageList = _repository.Get(t =>
            t.CreatedBy == iduser ||
            t.Channel.ChannelUsers.Any(tu => tu.UserId == iduser));
        return channelmessageList.FirstOrDefault(t => t.Id == id);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteChannelMessage(long id)
    {
        var channelmessage = GetChannelMessage(id);
        if(channelmessage == default){
            return new ErrorResponse("ChannelMessage is not found");
        }
        if(channelmessage.CreatedBy != new Guid(AuthController.GetUserId(HttpContext))){
            return new ErrorResponse("You can't delete this");
        }
        var result = _repository.Remove(channelmessage);
        _repository.Save();
        return new SuccessResponse<ChannelMessage>(result);
    }
}