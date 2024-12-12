using System.Linq.Expressions;
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
public class PermissionController : Controller
{
    private readonly IRepository<Permission> _repository;

    public PermissionController(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.Permissions;
    }

    [HttpPost]
    public ActionResult CreatePermission(Permission permission)
    {
        var entity = _repository.Add(permission);
        _repository.Save();
        return new SuccessResponse<Permission>(entity);
    }

    [HttpPut]
    public ActionResult UpdatePermission(Permission permission)
    {
        var entity = _repository.Update(permission);
        _repository.Save();
        return new SuccessResponse<Permission>(entity);
    }

    [HttpPatch("{id}")]
    public ActionResult UpdatePermission(long id, [FromBody] JsonPatchDocument<Permission> patchDoc)
    {
        return new SuccessResponse<Permission>(_repository.UpdatePatch(id, patchDoc));
    }

    [HttpDelete("{id}")]
    public ActionResult DeletePermission(long id)
    {
        var entity = _repository.GetById(id.ToString());
        _repository.Remove(entity);
        _repository.Save();
        return new SuccessResponse<Permission>(entity);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Permission>> Get([FromQuery(Name = "filter")] string? filterString, int? page,
        int? pageItem, string? includes = "")
    {
        var filter = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString)) filter = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        return new SuccessResponse<IEnumerable<Permission>>(
            _repository.Get(CompositeFilter<Permission>.ApplyFilter(filter), includeProperties: includes));
        
    }

    [HttpGet]
    [Route("{permissionId}")]
    public ActionResult<IEnumerable<Permission>> GetPermissionById(long permissionId, string? includes = "")
    {
        return new SuccessResponse<Permission>(_repository.GetById(permissionId.ToString(), includes));
    }
}