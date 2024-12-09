using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;

namespace server.Controllers.User;

[ApiController]
[Route("/user/[controller]")]
public class ChannelController: Controller
{

    private readonly IRepository<Profile> Users;
    private readonly IRepository<Channel> _repository;
    
    public ChannelController(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.Channels;
        Users = unitOfWork.Users;
    }
    
    [HttpGet]
    public ActionResult GetChannels()
    {
        Guid userId = new Guid(AuthController.GetUserId(HttpContext));
        var channelList = _repository.Get(t =>
            t.CreatedBy == userId ||
            t.ChannelUsers.Any(tu => tu.UserId == userId));
       
        return new SuccessResponse<IEnumerable<Channel>>(channelList);
    }

    [HttpPost]
    public ActionResult CreateChannel(Channel channel)
    {
        var id = AuthController.GetUserId(HttpContext);
        channel.CreatedBy = new Guid(id);
        var result = _repository.Add(channel);
        _repository.Save();
        return new SuccessResponse<Channel>(result);
    }

    [HttpPut]
    public ActionResult UpdateChannel(Channel Channel)
    {
        var id = AuthController.GetUserId(HttpContext);
        var channel = GetChannel(Channel.Id);
        if(channel == default){
            return new ErrorResponse("Channel is not found");
        }
        if(channel.CreatedBy != new Guid(id)){
            return new ErrorResponse("You can't change this");
        }
        if( channel.CreatedBy == new Guid(id) && (channel.Name != Channel.Name || 
        channel.CreatedBy != Channel.CreatedBy  || 
        channel.ChannelMessages != Channel.ChannelMessages || 
        channel.ChannelUsers != channel.ChannelUsers ||
        channel.DepartmentId != channel.DepartmentId)){
            return new ErrorResponse("You can't change this");
        }
        channel.ChannelMessages = null;
        channel.ChannelUsers = null;
        var result = _repository.Update(Channel);
        _repository.Save();
        return new SuccessResponse<Channel>(result);
    }

    public Channel GetChannel(long id){
        var iduser = new Guid(AuthController.GetUserId(HttpContext));
        var channelList = _repository.Get(t =>
            t.CreatedBy == iduser ||
            t.ChannelUsers.Any(tu => tu.UserId == iduser));
        return channelList.FirstOrDefault(t => t.Id == id);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteChannel(long id)
    {
        var channel = GetChannel(id);
        if(channel == default){
            return new ErrorResponse("Channel is not found");
        }
        if(channel.CreatedBy != new Guid(AuthController.GetUserId(HttpContext))){
            return new ErrorResponse("You can't delete this");
        }
        var result = _repository.Remove(channel);
        _repository.Save();
        return new SuccessResponse<Channel>(result);
    }
}