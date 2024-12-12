using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using server.Entities;
using server.Helpers;
using server.Interfaces;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class ChannelUserController : Controller
{
    private readonly IRepository<ChannelUser> _repository;

    public ChannelUserController(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.ChannelUsers;
    }

    [HttpPost]
    public ActionResult Create(ChannelUser body)
    {
        var entity = _repository.Add(body);
        _repository.Save();
        return new SuccessResponse<ChannelUser>(entity);
    }

    [HttpPut]
    public ActionResult Update(ChannelUser body)
    {
        var entity = _repository.Update(body);
        _repository.Save();
        return new SuccessResponse<ChannelUser>(entity);
    }

    [HttpPatch("{id}")]
    public ActionResult UpdatePatch(int id, [FromBody] JsonPatchDocument<ChannelUser> patchDoc)
    {
        return new SuccessResponse<ChannelUser>(_repository.UpdatePatch(id, patchDoc));
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteId(long id)
    {
        var entity = _repository.GetById(id.ToString());
        _repository.Remove(entity);
        _repository.Save();
        return new SuccessResponse<ChannelUser>(entity);
    }

    [HttpDelete]
    public ActionResult Delete(ChannelUser body)
    {
        _repository.Remove(body);
        _repository.Save();
        return new SuccessResponse<ChannelUser>(body);
    }


    [HttpGet]
    public ActionResult Get([FromQuery(Name = "filter")] string? filterString, int? page,
        int? pageItem, string? includes = "")
    {
        var filter = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString)) filter = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        return new SuccessResponse<IEnumerable<ChannelUser>>(
            _repository.Get(CompositeFilter<ChannelUser>.ApplyFilter(filter), includeProperties: includes));
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult GetId(long id, string? includes = "")
    {
        return new SuccessResponse<ChannelUser>(_repository.GetById(id.ToString(), includes));
    }
}