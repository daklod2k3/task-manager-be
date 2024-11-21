using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using System.Linq.Expressions;

namespace server.Interfaces;

public interface IDepartmentService
{
    IEnumerable<Department> GetAllDepartment();
    Department CreateDepartment(Department department);
    Department GetDepartmentById(long id);
    Department UpdateDepartmentById(long id, Department department);
    Department PatchDepartmentById(long id, [FromBody] JsonPatchDocument<Department> patchDoc);
    Department DeleteDepartmentById(long id);
}