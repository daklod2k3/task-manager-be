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
    private readonly IResourceService _resourceService;

    public ResourceController(IResourceService resourceService)
    {
        _resourceService = resourceService;
    }

    [HttpPost]
    public ActionResult CreateResource(Resource resource)
    {
        try
        {
            return new SuccessResponse<Resource>(_resourceService.CreateResource(resource));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Resource is not create");
        }
    }

    [HttpPut]
    public ActionResult UpdateResource(Resource resource)
    {
        try
        {
            return new SuccessResponse<Resource>(_resourceService.UpdateResource(resource));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Resource is not update");
        }
    }

    [HttpPatch("{id}")]
    public ActionResult UpdateResource(long id, [FromBody] JsonPatchDocument<Resource> patchDoc)
    {
        try
        {
            return new SuccessResponse<Resource>(_resourceService.UpdateResource(id, patchDoc));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("resource is not update");
        }
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteResource(long id)
    {
        try
        {
            return new SuccessResponse<Resource>(_resourceService.DeleteResource(id));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("resource is not delete");
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<Resource>> Get()
    {
        return new SuccessResponse<IEnumerable<Resource>>(_resourceService.GetAllResource());
        
    }

    [HttpGet]
    [Route("{resourceId}")]
    public ActionResult<IEnumerable<Resource>> GetResourceById(long resourceId)
    {
        try
        {
            return new SuccessResponse<Resource>(_resourceService.GetResource(resourceId));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("resource is not get");
        }
    }
}