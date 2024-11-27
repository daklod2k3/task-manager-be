using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Helpers;
using System.Linq.Expressions;

namespace server.Interfaces;

public interface IPermissionService
{
    IEnumerable<Permission> GetAllPermission();
    Permission CreatePermission(Permission permission);
    public Permission GetPermission(long id);
    Permission DeletePermission(long idPermission);
    Permission UpdatePermission(long id, [FromBody] JsonPatchDocument<Permission> patchDoc);
    Permission UpdatePermission(Permission permission);
}