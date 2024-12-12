using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;

namespace server.Controllers.User;

[ApiController]
[Route("/user/[controller]")]
public class NotificationController: Controller
{

    private readonly IRepository<Profile> Users;
    private readonly IRepository<Notification> _repository;
    
    public NotificationController(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.Notifications;
        Users = unitOfWork.Users;
    }
    
    [HttpGet]
    public ActionResult GetNotifications()
    {
        Guid userId = new Guid(AuthController.GetUserId(HttpContext));
        var notificationList = _repository.Get(t =>
            t.UserId == userId);
       
        return new SuccessResponse<IEnumerable<Notification>>(notificationList);
    }

    [HttpPost]
    public ActionResult CreateNotification(Notification notification)
    {
        var id = AuthController.GetUserId(HttpContext);
        var result = _repository.Add(notification);
        _repository.Save();
        return new SuccessResponse<Notification>(result);
    }

    [HttpPut]
    public ActionResult UpdateNotification(Notification Notification)
    {
        var id = AuthController.GetUserId(HttpContext);
        var notification = GetNotification(Notification.Id);
        if(notification == default){
            return new ErrorResponse("Notification is not found");
        }
        if(notification.UserId != new Guid(id)){
            return new ErrorResponse("You can't change this");
        }
        if( notification.UserId == new Guid(id) && 
        (notification.CreatedAt != Notification.CreatedAt || 
        notification.Content != Notification.Content)){
            return new ErrorResponse("You can't change this");
        }
        notification.User = null;
        var result = _repository.Update(Notification);
        _repository.Save();
        return new SuccessResponse<Notification>(result);
    }

    public Notification GetNotification(long id){
        var iduser = new Guid(AuthController.GetUserId(HttpContext));
        var notificationList = _repository.Get(t =>
            t.UserId == iduser);
        return notificationList.FirstOrDefault(t => t.Id == id);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteNotification(long id)
    {
        var notification = GetNotification(id);
        if(notification == default){
            return new ErrorResponse("Notification is not found");
        }
        if(notification.UserId != new Guid(AuthController.GetUserId(HttpContext))){
            return new ErrorResponse("You can't delete this");
        }
        var result = _repository.Remove(notification);
        _repository.Save();
        return new SuccessResponse<Notification>(result);
    }
}