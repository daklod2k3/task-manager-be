using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using server.Entities;
using server.Helpers;
using server.Interfaces;

namespace server.Controllers.AuthUser;

[ApiController]
[Route("auth/user/[controller]")]
public class ChannelMessageController : Controller
{
    private readonly IRepository<ChannelMessage> _repository;

    public ChannelMessageController(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.ChannelMessages;
    }

    [HttpPost]
    public ActionResult Create(ChannelMessage body)
    {
        body.CreatedBy = new Guid(AuthController.GetUserId(HttpContext));
        var entity = _repository.Add(body);
        _repository.Save();
        return new SuccessResponse<ChannelMessage>(entity);
    }

    [HttpPut]
    public ActionResult Update(ChannelMessage body)
    {
        var entity = _repository.Update(body);
        _repository.Save();
        return new SuccessResponse<ChannelMessage>(entity);
    }

    [HttpPatch("{id}")]
    public ActionResult UpdatePatch(int id, [FromBody] JsonPatchDocument<ChannelMessage> patchDoc)
    {
        return new SuccessResponse<ChannelMessage>(_repository.UpdatePatch(id.ToString(), patchDoc));
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteId(long id)
    {
        var entity = _repository.GetById(id.ToString());
        _repository.Remove(entity);
        _repository.Save();
        return new SuccessResponse<ChannelMessage>(entity);
    }

    [HttpDelete]
    public ActionResult Delete(ChannelMessage body)
    {
        _repository.Remove(body);
        _repository.Save();
        return new SuccessResponse<ChannelMessage>(body);
    }


    [HttpGet]
    public ActionResult Get([FromQuery(Name = "filter")] string? filterString, int? page,
        int? pageItem, string? includes = "")
    {
        var filter = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString)) filter = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        return new SuccessResponse<IEnumerable<ChannelMessage>>(
            _repository.Get(CompositeFilter<ChannelMessage>.ApplyFilter(filter), includes));
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult GetId(long id, string? includes = "")
    {
        return new SuccessResponse<ChannelMessage>(_repository.GetById(id.ToString(), includes));
    }
}