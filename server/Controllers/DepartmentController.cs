using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Graph.Models;
using Newtonsoft.Json;
using server.Entities;
using server.Helpers;
using server.Interfaces;
using server.Repository;
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
            return new SuccessResponse<Department>(_departmentService.UpdateDepartment(id, patchDoc));
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
    public ActionResult<IEnumerable<object>> GetDepartment(string? filterString, string? includeProperties, bool includeCompletionPercentage = true)
    {
        try
        {
            Expression<Func<Department, bool>>? filter = null;

            if (!string.IsNullOrEmpty(filterString))
            {
                var filterResult = JsonConvert.DeserializeObject<ClientFilter>(filterString);
                filter = CompositeFilter<Department>.ApplyFilter(filterResult);
            }

            var departmentList = _departmentService.GetDepartmentByFilter(filter, includeProperties);

            if (includeCompletionPercentage)
            {
                var resultWithCompletion = departmentList.Select(department => new
                {
                    Department = department,
                    CompletionPercentage = _departmentService.GetTaskCompletionPercentage(department.Id)
                });

                return new SuccessResponse<IEnumerable<object>>(resultWithCompletion);
            }

            return new SuccessResponse<IEnumerable<Department>>(departmentList);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new ErrorResponse("Failed to retrieve departments");
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<object>> Get(string? filter, string? includes)
    {
        return GetDepartment(filter, includes);
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<object> GetDepartmentById(long id, string? includes, bool includeCompletionPercentage = true)
    {
        try
        {
            var department = _departmentService.GetDepartment(id, includes);
            if (department == null)
            {
                return new ErrorResponse("Department not found");
            }

            if (includeCompletionPercentage)
            {
                var resultWithCompletion = new
                {
                    Department = department,
                    CompletionPercentage = _departmentService.GetTaskCompletionPercentage(department.Id)
                };

                return new SuccessResponse<object>(resultWithCompletion);
            }

            return new SuccessResponse<Department>(department);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Failed to get department");
        }
    }
}