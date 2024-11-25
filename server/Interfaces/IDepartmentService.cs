using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Helpers;
using System.Linq.Expressions;

namespace server.Interfaces;
    public interface IDepartmentService
    {
    public IEnumerable<Department> GetAllDepartment(Expression<Func<Department, bool>>? filter, string? includes);
    Department CreatDepartment(Department department);
    public Department GetDepartment(long idDepartment, string? includes);
    Department DeleteDepartment(long idDepartment);
    Department UpdateDepartment(long idDepartment, [FromBody] JsonPatchDocument<Department> patchDoc);
    Department UpdateDepartment(Department department);
    public IEnumerable<Department> GetDepartmentByFilter(Expression<Func<Department, bool>> compositeFilterExpression);
}
