using System.Linq.Expressions;
using LinqKit;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;

namespace server.Services;

public class DepartmentUserService : IDepartmentUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public DepartmentUserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public DepartmentUser CreateDepartmentUser(DepartmentUser departmentUser)
    {
        var result = _unitOfWork.DepartmentUser.Add(departmentUser);
        _unitOfWork.Save();
        return result;
    }

    public DepartmentUser GetDepartmentUser(long id)
    {
        return _unitOfWork.DepartmentUser.GetById(id);
    }

    public IEnumerable<DepartmentUser> GetAllDepartmentUser()
    {
        return _unitOfWork.DepartmentUser.Get();
    }

    public DepartmentUser UpdateDepartmentUser(DepartmentUser departmentUser)
    {
        var result = _unitOfWork.DepartmentUser.Update(departmentUser);
        _unitOfWork.Save();
        return result;
    }

    public DepartmentUser UpdateDepartmentUserPatch(long id, [FromBody] JsonPatchDocument<DepartmentUser> patchDoc)
    {

        var departmentUser = _unitOfWork.DepartmentUser.GetById(id);
        if (departmentUser == null)
        {
            throw new Exception("not found department");
        }

        patchDoc.ApplyTo(departmentUser);

        _unitOfWork.Save();

        return departmentUser;
    }

    public DepartmentUser DeleteDepartmentUser(long id)
    {
        var departmentUser = _unitOfWork.DepartmentUser.GetById(id);
        var result = _unitOfWork.DepartmentUser.Remove(departmentUser);
        _unitOfWork.Save();
        return result;
    }

    public IEnumerable<DepartmentUser> GetDepartmentUserByFilter(Expression<Func<DepartmentUser, bool>> filter)
    {
        return _unitOfWork.DepartmentUser.Get(filter);
    }
}