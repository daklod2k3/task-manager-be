using System.Linq.Expressions;
using LinqKit;
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
        return _unitOfWork.Department.Get(x => x.Id == id);
    }

    public IEnumerable<Department> GetAllDepartment()
    {
        CreatDepartment(new Department());
        return _unitOfWork.Department.GetAll();
    }

    public Department UpdateDepartment(Department department)
    {
        var result = _unitOfWork.Department.Update(department);
        _unitOfWork.Save();
        return result;
    }

    public Department UpdateDepartmentPatch(long id, [FromBody] JsonPatchDocument<Department> patchDoc)
    {

        var department = _unitOfWork.Department.Get(x => x.Id == id);
        if (department == null)
        {
            throw new Exception("not found department");
        }

        patchDoc.ApplyTo(department);

        _unitOfWork.Save();

        return department;
    }

    public Department DeleteDepartment(long id)
    {
        var department = _unitOfWork.Department.Get(x => x.Id == id);
        var result = _unitOfWork.Department.Remove(department);
        _unitOfWork.Save();
        return result;
    }

    public IEnumerable<Department> GetDepartmentByFilter(Expression<Func<Department, bool>> filter)
    {
        return _unitOfWork.Department.GetAll(filter, "DepartmentUsers,TaskDepartments");
    }
}