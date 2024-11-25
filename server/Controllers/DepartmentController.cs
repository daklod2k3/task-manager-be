using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using server.Entities;
using server.Helpers;
using server.Interfaces;
using server.Services;
using System.Linq.Expressions;

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

    [HttpDelete("{id}")]
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

    [ApiExplorerSettings(IgnoreApi = true)]
    public ActionResult<IEnumerable<Department>> GetDepartment(string? filterString, string? includeProperties)
    {
        var filterResult = new ClientFilter();
        Expression<Func<Department, bool>>? filter = null;

        if (!string.IsNullOrEmpty(filterString))
        {
            filterResult = JsonConvert.DeserializeObject<ClientFilter>(filterString);
            filter = CompositeFilter<Department>.ApplyFilter(filterResult);
        }

        var DepartmetnList = _departmentService.GetDepartmentAll(filter, includeProperties);
        return new SuccessResponse<IEnumerable<Department>>(DepartmetnList);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Department>> Get(string? includeProperties)
    {
        try
        {
            var departments = _departmentService.GetAllDepartment(includeProperties);
            return new SuccessResponse<IEnumerable<Department>>(departments);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Failed to retrieve departments");
        }
    }

    [HttpGet]
    [Route("{departmentId}")]
    public ActionResult<IEnumerable<Department>> GetDepartmentById(long departmentId, string? includes)
    {
        try
        {
            return new SuccessResponse<Department>(_departmentService.GetDepartment(departmentId, includes));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("department is not get");
        }
    }
}