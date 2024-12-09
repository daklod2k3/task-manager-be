using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;

namespace server.Controllers.User;

[ApiController]
[Route("/user/[controller]")]
public class DepartmentUserController: Controller
{

    private readonly IRepository<Profile> Users;
    private readonly IRepository<DepartmentUser> _repository;
    
    public DepartmentUserController(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.DepartmentUsers;
        Users = unitOfWork.Users;
    }
    
    [HttpGet]
    public ActionResult GetDepartmentUsers()
    {
        Guid userId = new Guid(AuthController.GetUserId(HttpContext));
        var departmentuserList = _repository.Get(t =>
            t.UserId == userId ||
            t.Department.DepartmentUsers.Any( tu => tu.UserId == userId));
       
        return new SuccessResponse<IEnumerable<DepartmentUser>>(departmentuserList);
    }

    [HttpPost]
    public ActionResult CreateDepartmentUser(DepartmentUser departmentuser)
    {
        var id = AuthController.GetUserId(HttpContext);
        departmentuser.OwnerType = EDepartmentOwnerType.Member;
        var result = _repository.Add(departmentuser);
        _repository.Save();
        return new SuccessResponse<DepartmentUser>(result);
    }

    [HttpPut]
    public ActionResult UpdateDepartmentUser(DepartmentUser DepartmentUser)
    {
        var id = AuthController.GetUserId(HttpContext);
        var departmentuser = GetDepartmentUser(DepartmentUser.Id);
        if(departmentuser == default){
            return new ErrorResponse("DepartmentUser is not found");
        }
        if(departmentuser.OwnerType != EDepartmentOwnerType.Owner 
        && departmentuser.DepartmentId != DepartmentUser.DepartmentId){
            return new ErrorResponse("You can't change this");
        }
        if( departmentuser.OwnerType == EDepartmentOwnerType.Owner 
        && departmentuser.DepartmentId == DepartmentUser.DepartmentId 
        && (departmentuser.CreatedAt != DepartmentUser.CreatedAt || 
            departmentuser.UserId != DepartmentUser.UserId  || 
            departmentuser.OwnerType != DepartmentUser.OwnerType || 
            departmentuser.DepartmentId != departmentuser.DepartmentId)){
                return new ErrorResponse("You can't change this");
            }
        departmentuser.Department = null;
        departmentuser.User = null;
        var result = _repository.Update(DepartmentUser);
        _repository.Save();
        return new SuccessResponse<DepartmentUser>(result);
    }

    public DepartmentUser GetDepartmentUser(long id){
        var iduser = new Guid(AuthController.GetUserId(HttpContext));
        var departmentuserList = _repository.Get(t =>
            t.UserId == iduser ||
            t.Department.DepartmentUsers.Any(tu => tu.UserId == iduser));
        return departmentuserList.FirstOrDefault(t => t.Id == id);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteDepartmentUser(long id)
    {
        var departmentuser = GetDepartmentUser(id);
        if(departmentuser == default){
            return new ErrorResponse("DepartmentUser is not found");
        }
        if(departmentuser.UserId != new Guid(AuthController.GetUserId(HttpContext))){
            return new ErrorResponse("You can't delete this");
        }
        var result = _repository.Remove(departmentuser);
        _repository.Save();
        return new SuccessResponse<DepartmentUser>(result);
    }
}