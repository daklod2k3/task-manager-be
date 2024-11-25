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
public class DepartmentUserController : Controller
{
    private readonly IDepartmentUserService _departmentUserService;

    public DepartmentUserController(IDepartmentUserService departmentUserService)
    {
        _departmentUserService = departmentUserService;
    }

    [HttpPost]
    public ActionResult CreateDepartmentUser(DepartmentUser departmentUser)
    {
        try
        {
            return new SuccessResponse<DepartmentUser>(_departmentUserService.CreateDepartmentUser(departmentUser));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("DepartmentUser is not create");
        }
    }

    [HttpPut]
    public ActionResult UpdateDepartmentUser(DepartmentUser departmentUser)
    {
        try
        {
            return new SuccessResponse<DepartmentUser>(_departmentUserService.UpdateDepartmentUser(departmentUser));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("DepartmentUser is not update");
        }
    }

    [HttpPatch("{id}")]
    public ActionResult UpdateDepartmentUserPatch(long id, [FromBody] JsonPatchDocument<DepartmentUser> patchDoc)
    {
        try
        {
            return new SuccessResponse<DepartmentUser>(_departmentUserService.UpdateDepartmentUserPatch(id, patchDoc));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("departmentUser is not update");
        }
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteDepartmentUser(long id)
    {
        try
        {
            return new SuccessResponse<DepartmentUser>(_departmentUserService.DeleteDepartmentUser(id));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("departmentUser is not delete");
        }
    }

    public ActionResult<IEnumerable<DepartmentUser>> GetDepartmentUserByFilter(string filterString)
    {
        var filterResult = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString))
            filterResult = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        var compositeFilterExpression = CompositeFilter<DepartmentUser>.ApplyFilter(filterResult);

        var DepartmetnUserList = _departmentUserService.GetDepartmentUserByFilter(compositeFilterExpression);
        return new SuccessResponse<IEnumerable<DepartmentUser>>(DepartmetnUserList);
    }

    [HttpGet]
    public ActionResult<IEnumerable<DepartmentUser>> Get(string? filter)
    {
        //var id = AuthController.GetUserId(HttpContext);
        return GetDepartmentUserByFilter(filter);
    }

    [HttpGet]
    [Route("{departmentId}")]
    public ActionResult<IEnumerable<DepartmentUser>> GetDepartmentUserById(long departmentId)
    {
        try
        {
            return new SuccessResponse<DepartmentUser>(_departmentUserService.GetDepartmentUser(departmentId));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("departmentUserr is not get");
        }
    }
}