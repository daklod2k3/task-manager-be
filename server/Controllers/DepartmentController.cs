using System.Linq.Expressions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using server.Entities;
using server.Helpers;
using server.Interfaces;

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

    //phuong thuc get
    [HttpGet]
    public IActionResult GetAllDepartment()
    {
        try
        {
            return new SuccessResponse<IEnumerable<Department>>(_departmentService.GetAllDepartment());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Department is not found");
        }
    }

    //phuong thuc get theo id
    [HttpGet("{id}")]
    public IActionResult GetDepartmentById(long id)
    {
        try
        {
            return new SuccessResponse<Department>(_departmentService.GetDepartmentById(id));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Department is not found");
        }
    }

    //phuong thuc post tao 1 department
    [HttpPost]
    public IActionResult CreateDepartment(Department department)
    {
        try
        {
            return new SuccessResponse<Department>(_departmentService.CreateDepartment(department));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Department is not create");
        }
    }
    //phuong thuc PUT update 1 department
    [HttpPut("{id}")]
    public IActionResult UpdateDepartmentById(long id, Department department)
    {
        try
        {
            return new SuccessResponse<Department>(_departmentService.UpdateDepartmentById(id, department));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Department is not update");
        }
    }
    

    //phong thuc PATCH update 1 department 
    [HttpPatch("{id}")]
    public IActionResult PatchDepartmentById(long id, [FromBody] JsonPatchDocument<Department> patchDoc)
    {
        try
        {
            var department = _departmentService.GetDepartmentById(id);
            patchDoc.ApplyTo(department);
            return new SuccessResponse<Department>(_departmentService.PatchDepartmentById(id, patchDoc));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Department is not update");
        }
    }
    //phuong thuc delete 1 department
    [HttpDelete("{id}")]
    public IActionResult DeleteDepartmentById(long id)
    {
        try
        {
            return new SuccessResponse<Department>(_departmentService.DeleteDepartmentById(id));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Department is not delete");
        }
    }
}