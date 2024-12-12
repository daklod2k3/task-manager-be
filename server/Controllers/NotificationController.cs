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
    private readonly IRepository<Notification> _repository;
    public NotificationController(IUnitOfWork _unitofwork)
    {
        _repository = _unitofwork.Notifications;
    }


    [HttpGet]
    public ActionResult<IEnumerable<Notification>> Get([FromQuery(Name = "filter")] string? filterString, int? page,
        int? pageItem, string? includes = "")
    {
        var filter = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString)) filter = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        return new SuccessResponse<IEnumerable<Notification>>(
            _repository.Get(CompositeFilter<Notification>.ApplyFilter(filter), includeProperties: includes));
    }

    [HttpGet("{id}")]
    public ActionResult<IEnumerable<Notification>> Get(long id, string? includes ="")
    {
        return new SuccessResponse<Notification>(_repository.GetById(id.ToString(), includes));
    }


    //tạo notification mới
    [HttpPost]
    public ActionResult<Notification> CreateNotification(Notification notification)
    {
        notification.UserId = new Guid(AuthController.GetUserId(HttpContext));
        var entity = _repository.Add(notification);
        _repository.Save();
        return new SuccessResponse<Notification>(notification);
    }



//gửi entity lên server, server trả về entity read = true
[HttpPut]
public ActionResult<Notification> UpdateNotification(Notification notification)
{
    var entity = _repository.Update(notification);
    _repository.Save();
    return new SuccessResponse<Notification>(entity);
}

[HttpPatch("{id}")]
public ActionResult<Notification> PatchNotification(long id, [FromBody]JsonPatchDocument<Notification> notification)
{
    return new SuccessResponse<Notification>(_repository.UpdatePatch(id, notification));
}

[HttpDelete]
    public ActionResult DeleteNotification(long id)
    {
        var entity = _repository.GetById(id.ToString());
        _repository.Remove(entity);
        _repository.Save();
        return new SuccessResponse<Notification>(entity);
    }
}
