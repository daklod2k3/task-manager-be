using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Helpers;
using server.Interfaces;

namespace server.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IUnitOfWork _unitOfWork;

    public DepartmentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Department CreatDepartment(Department department)
    {
        var result = _unitOfWork.Department.Add(department);
        _unitOfWork.Save();
        return result;
    }

    public Department GetDepartment(long id,string? includes)
    {
        var query = _unitOfWork.Department.GetQuery(x => x.Id == id, includes).Distinct();
        return query.FirstOrDefault();
    }

    public IEnumerable<Department> GetAllDepartment(string? includes)
    {
        return _unitOfWork.Department.Get(includeProperties: includes);
    }

    public Department UpdateDepartment(Department department)
    {
        var result = _unitOfWork.Department.Update(department);
        _unitOfWork.Save();
        return result;
    }

    public Department UpdateDepartmentPatch(long id, [FromBody] JsonPatchDocument<Department> patchDoc)
    {
        var department = _unitOfWork.Department.GetById(id);
        if (department == null) throw new Exception("not found department");

        patchDoc.ApplyTo(department);

        _unitOfWork.Save();

        return department;
    }

    public Department DeleteDepartment(long id)
    {
        var department = _unitOfWork.Department.GetById(id);
        var result = _unitOfWork.Department.Remove(department);
        _unitOfWork.Save();
        return result;
    }

    public IEnumerable<Department> GetDepartmentByFilter(Expression<Func<Department, bool>>? filter)
    {
        return _unitOfWork.Department.Get(filter);
    }

    public IEnumerable<Department> GetDepartmentAll(Expression<Func<Department, bool>>? filter,
        string? includeProperties)
    {
        var query = _unitOfWork.Department.GetQuery(filter, includeProperties).Distinct();
        return query;
    }

    public Department GetDepartmentById(long id, Expression<Func<Department, bool>>? filter,
        string? includeProperties)
    {
        return _unitOfWork.Department.GetById(id, includeProperties);
    }
}