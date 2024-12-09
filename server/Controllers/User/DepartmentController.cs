using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;

namespace server.Controllers.User;

[ApiController]
[Route("/user/[controller]")]
public class DepartmentController: Controller
{

    private readonly IRepository<Profile> Users;
    private readonly IRepository<DepartmentUser> DepartmentUsers;
    private readonly IRepository<Department> _repository;
    
    public DepartmentController(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.Departments;
        Users = unitOfWork.Users;
        DepartmentUsers = unitOfWork.DepartmentUsers;
    }
    
    [HttpGet]
    public ActionResult GetDepartments()
    {
        Guid userId = new Guid(AuthController.GetUserId(HttpContext));
        var departmentList = _repository.Get(t =>
            t.DepartmentUsers.Any(du => du.UserId == userId));
        return new SuccessResponse<IEnumerable<Department>>(departmentList);
    }

    [HttpPost]
    public ActionResult CreateDepartment(Department department)
    {
        var id = AuthController.GetUserId(HttpContext);
        var d = _repository.Add(department);
        DepartmentUsers.Add(new DepartmentUser() { DepartmentId = d.Id, UserId = new Guid(id), OwnerType = EDepartmentOwnerType.Owner });
        _repository.Save();
        DepartmentUsers.Save();
        return new SuccessResponse<Department>(d);
    }

    [HttpPut]
    public ActionResult UpdateDepartment(Department department)
    {
        var id = AuthController.GetUserId(HttpContext);
        if(_repository.GetById(department.Id) == null) return new ErrorResponse("Department not found");
        if(!department.DepartmentUsers.Any(du => du.UserId == new Guid(id) && du.OwnerType == EDepartmentOwnerType.Owner))
            return new ErrorResponse("You can't update this Department");
        var result = _repository.Update(department);
        _repository.Save();
        return new SuccessResponse<Department>(result);
    }

    [HttpPatch("{id}")]
    public ActionResult UpdateDepartment(long id, [FromBody] JsonPatchDocument<Department> patchDoc)
    {
        var iduser = AuthController.GetUserId(HttpContext);
        var department = _repository.GetById(id);
        if(_repository.GetById(department.Id) == null) return new ErrorResponse("Department not found");
        if(!department.DepartmentUsers.Any(du => du.UserId == new Guid(iduser) && du.OwnerType == EDepartmentOwnerType.Owner))
            return new ErrorResponse("You can't update this Department");
        var result = _repository.UpdatePatch(id, patchDoc);
        _repository.Save();
        return new SuccessResponse<Department>(result);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteDepartment(long id)
    {
        var iduser = AuthController.GetUserId(HttpContext);
        var department = _repository.GetById(id);
        if(_repository.GetById(department.Id) == null) return new ErrorResponse("Department not found");
        if(!department.DepartmentUsers.Any(du => du.UserId == new Guid(iduser) && du.OwnerType == EDepartmentOwnerType.Owner))
            return new ErrorResponse("You can't delete this Department");
        var result = _repository.Remove(department);
        _repository.Save();
        return new SuccessResponse<Department>(result);
    }
}