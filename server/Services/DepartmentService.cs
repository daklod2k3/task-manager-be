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

    public Department GetDepartment(long idDepartment, string? includes)
    {
        var query = _unitOfWork.Department.GetQuery(x => x.Id == idDepartment, includes);

        return query.FirstOrDefault();
    }

    public IEnumerable<Department> GetAllDepartment(Expression<Func<Department, bool>>? filter, string? includeProperties)
    {
        filter ??= d => true;

        var query = _unitOfWork.Department.GetQuery(filter, includeProperties)
            .Distinct();

        return query.ToList();
    }

    public Department DeleteDepartment(long id)
    {
        var department = _unitOfWork.Department.GetById(id);
        var result = _unitOfWork.Department.Remove(department);
        _unitOfWork.Save();
        return result;
    }

    public Department UpdateDepartment(long idDepartment, [FromBody] JsonPatchDocument<Department> patchDoc)
    {
        var department = _unitOfWork.Department.GetById(idDepartment);
        if (department == null) throw new Exception("not found department");

        patchDoc.ApplyTo(department);

        _unitOfWork.Save();

        return department;
    }

    public IEnumerable<Department> GetDepartmentByFilter(Expression<Func<Department, bool>> filter)
    {
        return _unitOfWork.Department.Get(filter);
    }

    public Department UpdateDepartment(Department department)
    {
        var result = _unitOfWork.Department.Update(department);
        _unitOfWork.Save();
        return result;
    }
}