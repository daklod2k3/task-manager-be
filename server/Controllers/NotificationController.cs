using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using server.Context;
using server.Entities;
using server.Interfaces;
using Newtonsoft.Json;
using server.Helpers;
using Microsoft.AspNetCore.JsonPatch;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationController : Controller
{
    private readonly INotificationService _notificationService;
    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    

    //lấy mọi notifications
    // [HttpGet]
    // public ActionResult<IEnumerable<Notification>> Get()
    // {
    //     return _notificationService.GetAllNotifications().ToList();
        
    // }

    [HttpGet]
    public ActionResult<IEnumerable<Notification>> Get([FromQuery(Name = "filter")] string? filterString, string? includes = "")
    {
        var filter = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString)) filter = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        return new SuccessResponse<IEnumerable<Notification>>(
            _notificationService.GetNotificationByFilter(CompositeFilter<Notification>.ApplyFilter(filter), includes));
    }

    [HttpGet("{id}")]
    public ActionResult<IEnumerable<Notification>> Get(string id)
    {
        try
        {
            return new SuccessResponse<Notification>(_notificationService.GetNotificationById(new Guid(id)));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("notification is not get");
        }
    }


    //tạo notification mới
    [HttpPost]
    public ActionResult<Notification> CreateNotification(Notification notification)
    {
        try
        {
            return new SuccessResponse<Notification>(_notificationService.CreateNotification(notification));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Notification failed to create");
        }
    }



//gửi entity lên server, server trả về entity read = true
[HttpPut]
public ActionResult<Notification> UpdateNotification(Notification notification)
{
    try
        {
            return new SuccessResponse<Notification>(_notificationService.UpdateNotification(notification));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Notification did not update");
        }
}

[HttpPatch("{id}")]
public ActionResult<Notification> PatchNotification(long id, [FromBody]JsonPatchDocument<Notification> notification)
{
    try
        {
            return new SuccessResponse<Notification>(_notificationService.PatchNotification(id,notification));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Notification did not patch");
        }
}

[HttpDelete]
    public ActionResult DeleteNotification(long id)
    {
        try
        {
            return new SuccessResponse<Notification>(_notificationService.DeleteNotification(id));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Notification did not delete");
        }
    }
}
