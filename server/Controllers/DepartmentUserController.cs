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

public class DepartmentUserController : Controller 
{
    private readonly IDepartmentUserService _departmentUserService;
    public DepartmentUserController(IDepartmentUserService departmentUserService)
    {
        _departmentUserService = departmentUserService;
    }

    //phuong thuc get
    [HttpGet]
    public IActionResult GetAllDepartmentUser()
    {
        try
        {
            return new SuccessResponse<IEnumerable<DepartmentUser>>(_departmentUserService.GetAllDepartmentUser());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("DepartmentUser is not found");
        }
    }

    //phuong thuc get theo id
    [HttpGet("{id}")]
    public IActionResult GetDepartmentUserById(long id)
    {
        try
        {
            return new SuccessResponse<DepartmentUser>(_departmentUserService.GetDepartmentUserById(id));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("DepartmentUser is not found");
        }
    }

    //phuong thuc post tao 1 departmentuser
    [HttpPost]
    public IActionResult CreateDepartmentUser(DepartmentUser departmentUser)
    {
        try
        {
            return new SuccessResponse<DepartmentUser>(_departmentUserService.CreateDepartmentUser(departmentUser));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("DepartmentUser is not found");
        }
    }

    //phuong thuc put update 1 departmentuser
    [HttpPut("{id}")]
    public IActionResult UpdateDepartmentUserById(long id, DepartmentUser departmentUser)
    {
        try
        {
            return new SuccessResponse<DepartmentUser>(_departmentUserService.UpdateDepartmentUserById(id, departmentUser));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("DepartmentUser is not found");
        }
    }

    //phuong thuc patch update 1 departmentuser
    [HttpPatch("{id}")]
    public IActionResult PatchDepartmentUserById(long id, [FromBody] JsonPatchDocument<DepartmentUser> departmentUserPatch)
    {
        try
        {
            var departmentUser = _departmentUserService.GetDepartmentUserById(id);
            departmentUserPatch.ApplyTo(departmentUser);
            return new SuccessResponse<DepartmentUser>(_departmentUserService.UpdateDepartmentUserById(id, departmentUser));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("DepartmentUser is not found");
        }
    }

    //phuong thuc delete 1 departmentuser
    [HttpDelete("{id}")]
    public IActionResult DeleteDepartmentUserById(long id)
    {
        try
        {
            return new SuccessResponse<DepartmentUser>(_departmentUserService.DeleteDepartmentUserById(id));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("DepartmentUser is not found");
        }
    }
}