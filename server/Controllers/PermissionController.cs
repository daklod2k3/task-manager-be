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
    private readonly IPermissionService _permissionService;

    public PermissionController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    [HttpPost]
    public ActionResult CreatePermission(Permission permission)
    {
        try
        {
            return new SuccessResponse<Permission>(_permissionService.CreatePermission(permission));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Permission is not create");
        }
    }

    [HttpPut]
    public ActionResult UpdatePermission(Permission permission)
    {
        try
        {
            return new SuccessResponse<Permission>(_permissionService.UpdatePermission(permission));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Permission is not update");
        }
    }

    [HttpPatch("{id}")]
    public ActionResult UpdatePermission(long id, [FromBody] JsonPatchDocument<Permission> patchDoc)
    {
        try
        {
            return new SuccessResponse<Permission>(_permissionService.UpdatePermission(id, patchDoc));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("permission is not update");
        }
    }

    [HttpDelete("{id}")]
    public ActionResult DeletePermission(long id)
    {
        try
        {
            return new SuccessResponse<Permission>(_permissionService.DeletePermission(id));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("permission is not delete");
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<Permission>> Get(string? filterString)
    {
        Expression<Func<Permission, bool>>? filter = null;
        var filterResult = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString))
        {
            
            filterResult = JsonConvert.DeserializeObject<ClientFilter>(filterString);
            filter = CompositeFilter<Permission>.ApplyFilter(filterResult);
        }
        return new SuccessResponse<IEnumerable<Permission>>(_permissionService.GetPermissionByFilter(filter));
        
    }

    [HttpGet]
    [Route("{permissionId}")]
    public ActionResult<IEnumerable<Permission>> GetPermissionById(long permissionId)
    {
        try
        {
            return new SuccessResponse<Permission>(_permissionService.GetPermission(permissionId));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("permission is not get");
        }
    }
}