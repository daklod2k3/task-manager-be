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
public class DepartmentController : Controller
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpPost]
    public ActionResult CreateDepartment(Department department)
    {
        try
        {
            return new SuccessResponse<Department>(_departmentService.CreatDepartment(department));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Department is not create");
        }
    }

    [HttpPut]
    public ActionResult UpdateDepartment(Department department)
    {
        try
        {
            return new SuccessResponse<Department>(_departmentService.UpdateDepartment(department));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Department is not update");
        }
    }

    [HttpPatch("{id}")]
    public ActionResult UpdateDepartmentPatch(long id, [FromBody] JsonPatchDocument<Department> patchDoc)
    {
        try
        {
            return new SuccessResponse<Department>(_departmentService.UpdateDepartmentPatch(id, patchDoc));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("department is not update");
        }
    }

    [HttpDelete]
    public ActionResult DeleteDepartment(long id)
    {
        try
        {
            return new SuccessResponse<Department>(_departmentService.DeleteDepartment(id));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("department is not delete");
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public ActionResult<IEnumerable<Department>> GetDepartmentByFilter(string filterString)
    {
        var filterResult = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString))
            filterResult = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        var compositeFilterExpression = CompositeFilter<Department>.ApplyFilter(filterResult);

        var DepartmetnList = _departmentService.GetDepartmentByFilter(compositeFilterExpression);
        return new SuccessResponse<IEnumerable<Department>>(DepartmetnList);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Department>> Get(string? filter)
    {
        //var id = AuthController.GetUserId(HttpContext);
        return GetDepartmentByFilter(filter);
    }

    [HttpGet]
    [Route("{departmentId}")]
    public ActionResult<IEnumerable<Department>> GetDepartmentById(long departmentId)
    {
        try
        {
            return new SuccessResponse<Department>(_departmentService.GetDepartment(departmentId));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("department is not get");
        }
    }
}