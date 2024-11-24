
using server.Entities;
using Microsoft.AspNetCore.JsonPatch;
namespace server.Interfaces
{
    public interface IDepartmentUserService
    {
        IEnumerable<DepartmentUser> GetAll();
        DepartmentUser GetById(long id);
        DepartmentUser Create(DepartmentUser departmentUser);
        DepartmentUser Update(DepartmentUser departmentUser);
        DepartmentUser UpdatePatch(long id, Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<DepartmentUser> patch);
        DepartmentUser Delete(long id);
    }

}
