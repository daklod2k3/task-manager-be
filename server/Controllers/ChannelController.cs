using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using server.Entities;
using server.Helpers;
using server.Interfaces;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class ChannelController : Controller
{
    private readonly IRepository<Channel> _repository;

    public ChannelController(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.Channels;
    }

    [HttpPost]
    public ActionResult Create(Channel body)
    {
        body.CreatedBy = new Guid(AuthController.GetUserId(HttpContext));
        var entity = _repository.Add(body);
        _repository.Save();
        return new SuccessResponse<Channel>(entity);
    }

    [HttpPut]
    public ActionResult Update(Channel body)
    {
        var entity = _repository.Update(body);
        _repository.Save();
        return new SuccessResponse<Channel>(entity);
    }

    [HttpPatch("{id}")]
    public ActionResult UpdatePatch(int id, [FromBody] JsonPatchDocument<Channel> patchDoc)
    {
        return new SuccessResponse<Channel>(_repository.UpdatePatch(id, patchDoc));
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteId(long id)
    {
        var entity = _repository.GetById(id.ToString());
        _repository.Remove(entity);
        _repository.Save();
        return new SuccessResponse<Channel>(entity);
    }

    [HttpDelete]
    public ActionResult Delete(Channel body)
    {
        _repository.Remove(body);
        _repository.Save();
        return new SuccessResponse<Channel>(body);
    }


    [HttpGet]
    public ActionResult Get([FromQuery(Name = "filter")] string? filterString, int? page,
        int? pageItem, string? includes = "")
    {
        var filter = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString)) filter = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        return new SuccessResponse<IEnumerable<Channel>>(
            _repository.Get(CompositeFilter<Channel>.ApplyFilter(filter), includeProperties: includes));
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult GetId(long id, string? includes = "")
    {
        return new SuccessResponse<Channel>(_repository.GetById(id.ToString(), includes));
    }
}