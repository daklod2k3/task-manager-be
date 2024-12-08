using System.Linq.Expressions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;

namespace server.Services;

public class PermissionService : IPermissionService
{
    private readonly IUnitOfWork _unitOfWork;

    public PermissionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Permission CreatePermission(Permission permission)
    {
        var result = _unitOfWork.Permissions.Add(permission);
        _unitOfWork.Save();
        return result;
    }

    public Permission GetPermission(long id)
    {
        return _unitOfWork.Permissions.GetById(id);
    }

    public Permission UpdatePermission(Permission permission)
    {
        var result = _unitOfWork.Permissions.Update(permission);
        _unitOfWork.Save();
        return result;
    }

    public Permission UpdatePermission(long id, [FromBody] JsonPatchDocument<Permission> patchDoc)
    {
        var permission = _unitOfWork.Permissions.GetById(id);
        if (permission == null) throw new Exception("not found permission");

        patchDoc.ApplyTo(permission);

        _unitOfWork.Save();

        return permission;
    }

    public Permission DeletePermission(long id)
    {
        var permission = _unitOfWork.Permissions.GetById(id);
        var result = _unitOfWork.Permissions.Remove(permission);
        _unitOfWork.Save();
        return result;
    }

    public IEnumerable<Permission> GetAllPermission()
    {
        return _unitOfWork.Permissions.Get();
    }

    public IEnumerable<Permission> GetPermissionByFilter(Expression<Func<Permission, bool>> filter)
    {
        return _unitOfWork.Permissions.Get(filter);
    }

}