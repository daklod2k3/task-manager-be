using System.Linq.Expressions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
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

    public Department GetDepartment(long id)
    {
        return _unitOfWork.Department.GetById(id);
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

    public IEnumerable<Department> GetDepartmentByFilter(Expression<Func<Department, bool>> filter)
    {
        return _unitOfWork.Department.Get(filter, includeProperties: "DepartmentUsers,TaskDepartments");
    }

    public IEnumerable<Department> GetAllDepartment()
    {
        return _unitOfWork.Department.Get();
    }
}
