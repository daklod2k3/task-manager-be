using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using server.Entities;
using server.Helpers;
using server.Interfaces;
using server.Services;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class ResourceController : Controller
{
    private readonly IRepository<Resource> _repository;

    public ResourceController(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.Resources;
    }

    [HttpPost]
    public ActionResult CreateResource(Resource resource)
    {
        var entity = _repository.Add(resource);
        _repository.Save();
        return new SuccessResponse<Resource>(entity);
    }

    [HttpPut]
    public ActionResult UpdateResource(Resource resource)
    {
        var entity = _repository.Update(resource);
        _repository.Save();
        return new SuccessResponse<Resource>(entity);
    }

    [HttpPatch("{id}")]
    public ActionResult UpdateResource(long id, [FromBody] JsonPatchDocument<Resource> patchDoc)
    {
        return new SuccessResponse<Resource>(_repository.UpdatePatch(id, patchDoc));
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteResource(long id)
    {
        var entity = _repository.GetById(id.ToString());
        _repository.Remove(entity);
        _repository.Save();
        return new SuccessResponse<Resource>(entity);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Resource>> Get([FromQuery(Name = "filter")] string? filterString, int? page,
        int? pageItem, string? includes = "")
    {
        var filter = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString)) filter = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        return new SuccessResponse<IEnumerable<Resource>>(
            _repository.Get(CompositeFilter<Resource>.ApplyFilter(filter), includeProperties: includes));
        
    }

    [HttpGet]
    [Route("{resourceId}")]
    public ActionResult<IEnumerable<Resource>> GetResourceById(long resourceId, string? includes = "")
    {
        return new SuccessResponse<Resource>(_repository.GetById(resourceId.ToString(), includes));
    }
}