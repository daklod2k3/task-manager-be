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
    public Department CreateDepartment(Department department)
    {
        var result = _unitOfWork.Department.Add(department);
        _unitOfWork.Save();
        return result;
    }

    public Department DeleteDepartmentById(long id)
    {
        var department = _unitOfWork.Department.Get(x => x.Id == id);
        var result = _unitOfWork.Department.Remove(department);
        _unitOfWork.Save();
        return result;
    }

    public IEnumerable<Department> GetAllDepartment()
    {
        return _unitOfWork.Department.GetAll();
    }

    public Department GetDepartmentById(long id)
    {
        return _unitOfWork.Department.Get(x => x.Id == id);
    }

    public Department PatchDepartmentById(long id, [FromBody] JsonPatchDocument<Department> patchDoc)
    {
        var department = _unitOfWork.Department.Get(x => x.Id == id);
        if (department == null) throw new Exception("not found department");
        patchDoc.ApplyTo(department);
        _unitOfWork.Save();
        return department;
    }

    public Department UpdateDepartmentById(long id, Department department)
    {
        var departmentInDb = _unitOfWork.Department.Get(x => x.Id == id);
        if (departmentInDb == null) throw new Exception("not found department");
        departmentInDb.Name = department.Name;
        departmentInDb.CreatedAt = department.CreatedAt;
        _unitOfWork.Save();
        return departmentInDb;
    }
}