using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using System.Linq.Expressions;

namespace server.Interfaces;

public interface IDepartmentUserService
{
    IEnumerable<DepartmentUser> GetAllDepartmentUser();
    DepartmentUser CreateDepartmentUser(DepartmentUser departmentUser);
    DepartmentUser GetDepartmentUserById(long id);
    DepartmentUser UpdateDepartmentUserById(long id, DepartmentUser departmentUser);
    DepartmentUser PatchDepartmentUserById(long id, [FromBody] JsonPatchDocument<DepartmentUser> patchDoc);
    DepartmentUser DeleteDepartmentUserById(long id);
}