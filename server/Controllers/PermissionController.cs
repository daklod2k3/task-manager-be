using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using server.Entities;
using server.Helpers;
using server.Interfaces;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class PermissionController : Controller
{
    private readonly IRepository<Permission> _repository;

    public PermissionController(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.Permissions;
    }

    [HttpPost]
    public ActionResult Create(Permission role)
    {
        try
        {
            var entity = _repository.Add(role);
            _repository.Save();
            return new SuccessResponse<Permission>(entity);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }

    [HttpPut]
    public ActionResult Update(Permission body)
    {
        try
        {
            var role = _repository.Update(body);
            _repository.Save();
            return new SuccessResponse<Permission>(role);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }

    [HttpPatch("{id}")]
    public ActionResult UpdatePatch(long id, [FromBody] JsonPatchDocument<Permission> patchDoc)
    {
        return new SuccessResponse<Permission>(_repository.UpdatePatch(id, patchDoc));
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteId(long id)
    {
        try
        {
            var entity = _repository.GetById(id);
            _repository.Remove(entity);
            _repository.Save();
            return new SuccessResponse<Permission>(entity);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }

    [HttpDelete]
    public ActionResult Delete(Permission body)
    {
        try
        {
            _repository.Remove(body);
            _repository.Save();
            return new SuccessResponse<Permission>(body);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }


    [HttpGet]
    public ActionResult Get([FromQuery(Name = "filter")] string? filterString,
        string? includes, int? page, int? pageItem)
    {
        var filter = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString)) filter = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        return new SuccessResponse<IEnumerable<Permission>>(
            _repository.Get(CompositeFilter<Permission>.ApplyFilter(filter), includes));
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<IEnumerable<Permission>> GetId(long id, string? includes)
    {
        try
        {
            return new SuccessResponse<Permission>(_repository.GetById(id.ToString(), includes));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse(ex.ToString());
        }
    }
}