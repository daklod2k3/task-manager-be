using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using server.Entities;
using server.Helpers;
using server.Interfaces;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserMessageController : Controller
{
    private readonly IRepository<UserMessage> _repository;

    public UserMessageController(IUnitOfWork unitOfWork)
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
        return new SuccessResponse<UserMessage>(_repository.UpdatePatch(id.ToString(), patchDoc));
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
    public ActionResult Get([FromQuery(Name = "filter")] string? filterString, int? page,
        int? pageItem, string? includes = "")
    {
        var filter = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString)) filter = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        return new SuccessResponse<IEnumerable<UserMessage>>(
            _repository.Get(CompositeFilter<UserMessage>.ApplyFilter(filter), includeProperties: includes));
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult GetId(long id, string? includes = "")
    {
        return new SuccessResponse<UserMessage>(_repository.GetById(id.ToString(), includes));
    }
}