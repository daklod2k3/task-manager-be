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

    public DepartmentUser DeleteDepartmentUserById(long id)
    {
        var departmentUser = _unitOfWork.DepartmentUser.Get(x => x.Id == id);
        var result = _unitOfWork.DepartmentUser.Remove(departmentUser);
        _unitOfWork.Save();
        return result;
    }

    public IEnumerable<DepartmentUser> GetAllDepartmentUser()
    {
        return _unitOfWork.DepartmentUser.GetAll();
    }

    public DepartmentUser GetDepartmentUserById(long id)
    {
        return _unitOfWork.DepartmentUser.Get(x => x.Id == id);
    }

    public DepartmentUser PatchDepartmentUserById(long id, [FromBody] JsonPatchDocument<DepartmentUser> patchDoc)
    {
        var departmentUser = _unitOfWork.DepartmentUser.Get(x => x.Id == id);
        if (departmentUser == null) throw new Exception("not found departmentUser");
        patchDoc.ApplyTo(departmentUser);
        _unitOfWork.Save();
        return departmentUser;
    }

    public DepartmentUser UpdateDepartmentUserById(long id, DepartmentUser departmentUser)
    {
        var departmentUserInDb = _unitOfWork.DepartmentUser.Get(x => x.Id == id);
        if (departmentUserInDb == null) throw new Exception("not found departmentUser");
        departmentUserInDb.DepartmentId = departmentUser.DepartmentId;
        departmentUserInDb.UserId = departmentUser.UserId;
        departmentUserInDb.CreatedAt = departmentUser.CreatedAt;
        _unitOfWork.Save();
        return departmentUserInDb;
    }
}