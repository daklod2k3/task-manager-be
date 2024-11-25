using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using System.Linq.Expressions;

namespace server.Interfaces;
public interface IDepartmentUserService
{
    public DepartmentUser CreateDepartmentUser(DepartmentUser DepartmentUser);
    public DepartmentUser UpdateDepartmentUser(DepartmentUser DepartmentUser);
    public DepartmentUser DeleteDepartmentUser(long idDepartmentUser);
    public DepartmentUser UpdateDepartmentUserPatch(long id, [FromBody] JsonPatchDocument<DepartmentUser> patchDoc);
    public DepartmentUser GetDepartmentUser(long id);
    public IEnumerable<DepartmentUser> GetDepartmentUserByFilter(Expression<Func<DepartmentUser, bool>> compositeFilterExpression);

}
