using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using server.Entities;
using server.Helpers;
using server.Interfaces;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    private readonly IRepository<Profile> _repository;

    public UserController(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.Users;
    }

    [HttpPost]
    public ActionResult Create(Profile body)
    {
        var entity = _repository.Add(body);
        _repository.Save();
        return new SuccessResponse<Profile>(entity);
    }

    [HttpPut]
    public ActionResult Update(Profile body)
    {
        var entity = _repository.Update(body);
        _repository.Save();
        return new SuccessResponse<Profile>(entity);
    }

    [HttpPatch("{id}")]
    public ActionResult UpdatePatch(string id, [FromBody] JsonPatchDocument<Profile> patchDoc)
    {
        if (!Guid.TryParse(id, out Guid guidId))
        {
            return BadRequest(new ErrorResponse("Invalid ID format."));
        }
        return new SuccessResponse<Profile>(_repository.UpdatePatch(guidId, patchDoc));
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteId(long id)
    {
        var entity = _repository.GetById(id.ToString());
        _repository.Remove(entity);
        _repository.Save();
        return new SuccessResponse<Profile>(entity);
    }

    [HttpDelete]
    public ActionResult Delete(Profile body)
    {
        _repository.Remove(body);
        _repository.Save();
        return new SuccessResponse<Profile>(body);
    }


    [HttpGet]
    public ActionResult Get([FromQuery(Name = "filter")] string? filterString, int? page,
        int? pageItem, string? includes = "")
    {
        var filter = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString)) filter = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        return new SuccessResponse<IEnumerable<Profile>>(
            _repository.Get(CompositeFilter<Profile>.ApplyFilter(filter), includes));
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult GetId(Guid id, string? includes = "")
    {
        return new SuccessResponse<Profile>(_repository.GetById(id, includes));
    }
}