using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Helpers;
using System.Linq.Expressions;

namespace server.Interfaces;
    public interface IDepartmentService
    {
    IEnumerable<Department> GetAllDepartment(string? includeProperties);
    Department CreatDepartment(Department department);
    public Department GetDepartment(long id, string? includes);
    Department DeleteDepartment(long idDepartment);
    Department UpdateDepartment(Department department);
    Department UpdateDepartmentPatch(long id, [FromBody] JsonPatchDocument<Department> patchDoc);
    public IEnumerable<Department> GetDepartmentAll(Expression<Func<Department, bool>> compositeFilterExpression, string? includeProperties);
    public IEnumerable<Department> GetDepartmentByFilter(Expression<Func<Department, bool>> compositeFilterExpression);
    public Department GetDepartmentById(long id, Expression<Func<Department, bool>>? compositeFilterExpression, string? includeProperties);

    }
