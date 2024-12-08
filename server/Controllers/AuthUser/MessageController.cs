using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using server.Entities;
using server.Helpers;
using server.Interfaces;

namespace server.Controllers.AuthUser;

[ApiController]
[Route("auth/user/[controller]")]
public class MessageController : Controller
{
    private readonly IUserMessageRepository _repository;

    public MessageController(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.UserMessages;
    }

    [HttpPost]
    public ActionResult Create(UserMessage body)
    {
        body.FromId = new Guid(AuthController.GetUserId(HttpContext));
        var entity = _repository.Add(body);
        _repository.Save();
        return new SuccessResponse<UserMessage>(entity);
    }

    [HttpPut]
    public ActionResult Update(UserMessage body)
    {
        var entity = _repository.Update(body);
        _repository.Save();
        return new SuccessResponse<UserMessage>(entity);
    }

    [HttpPatch("{id}")]
    public ActionResult UpdatePatch(int id, [FromBody] JsonPatchDocument<UserMessage> patchDoc)
    {
        return new SuccessResponse<UserMessage>(_repository.UpdatePatch(id, patchDoc));
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteId(long id)
    {
        var entity = _repository.GetById(id.ToString());
        _repository.Remove(entity);
        _repository.Save();
        return new SuccessResponse<UserMessage>(entity);
    }

    [HttpDelete]
    public ActionResult Delete(UserMessage body)
    {
        _repository.Remove(body);
        _repository.Save();
        return new SuccessResponse<UserMessage>(body);
    }


    [HttpGet]
    public ActionResult Get([FromQuery(Name = "filter")] string? filterString, string? orderBy, int? page,
        int? pageSize, string? includes = "")
    {
        var user_id = new Guid(AuthController.GetUserId(HttpContext));
        var filter = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString)) filter = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        var list = _repository.GetUserMessageList(
                user_id,
                _repository.GetQuery(CompositeFilter<UserMessage>.ApplyFilter(filter), includes, orderBy, page,
                    pageSize))
            .ToList();

        foreach (var userMessage in list)
            userMessage.SendTo = userMessage.FromId != user_id ? userMessage.From : userMessage.To;
        return new SuccessResponse<IEnumerable<UserMessage>>(
            list
        );
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult GetId(Guid id, [FromQuery(Name = "filter")] string? filterString, string? orderBy, int? page,
        int? pageSize, string? includes = "")
    {
        var user_id = new Guid(AuthController.GetUserId(HttpContext));
        return new SuccessResponse<IEnumerable<UserMessage>>(_repository
            .GetDirectMessages(user_id, id, _repository.GetQuery(null, includes, orderBy, page, pageSize)).ToList());
    }
}