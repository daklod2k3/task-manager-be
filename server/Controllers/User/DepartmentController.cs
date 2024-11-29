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
    private IUnitOfWork _unitOfWork;
    
    public DepartmentController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        Users = _unitOfWork.User;
    }
    
    [HttpGet]
    public ActionResult GetDepartments()
    {
        Guid userId = new Guid(AuthController.GetUserId(HttpContext));
        var departmentList = _unitOfWork.Department.Get(t =>
            t.DepartmentUsers.Any(du => du.UserId == userId));
       
        return new SuccessResponse<IEnumerable<Department>>(departmentList);
    }

    [HttpPost]
    public ActionResult CreateDepartment(Department department)
    {
        var id = AuthController.GetUserId(HttpContext);
        var d = _unitOfWork.Department.Add(department);
        _unitOfWork.DepartmentUser.Add(new DepartmentUser() { DepartmentId = d.Id, UserId = new Guid(id), OwnerType = EDepartmentOwnerType.Owner });
        return new SuccessResponse<Department>(d);
    }

    [HttpPut]
    public ActionResult UpdateDepartment(Department department)
    {
        var id = AuthController.GetUserId(HttpContext);
        if(_unitOfWork.Department.GetById(department.Id) == null) return new ErrorResponse("Department not found");
        if(!department.DepartmentUsers.Any(du => du.UserId == new Guid(id) && du.OwnerType == EDepartmentOwnerType.Owner))
            return new ErrorResponse("You can't update this Department");
        return new SuccessResponse<Department>(_unitOfWork.Department.Update(department));
    }

    [HttpPatch("{id}")]
    public ActionResult UpdateDepartment(long id, [FromBody] JsonPatchDocument<Department> patchDoc)
    {
        var iduser = AuthController.GetUserId(HttpContext);
        var department = _unitOfWork.Department.GetById(id);
        if(_unitOfWork.Department.GetById(department.Id) == null) return new ErrorResponse("Department not found");
        if(!department.DepartmentUsers.Any(du => du.UserId == new Guid(iduser) && du.OwnerType == EDepartmentOwnerType.Owner))
            return new ErrorResponse("You can't update this Department");
        return new SuccessResponse<Department>(_unitOfWork.Department.UpdatePatch(id.ToString(), patchDoc));
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteTask(long id)
    {
        var iduser = AuthController.GetUserId(HttpContext);
        var department = _unitOfWork.Department.GetById(id);
        if(_unitOfWork.Department.GetById(department.Id) == null) return new ErrorResponse("Department not found");
        if(!department.DepartmentUsers.Any(du => du.UserId == new Guid(iduser) && du.OwnerType == EDepartmentOwnerType.Owner))
            return new ErrorResponse("You can't update this Department");
        return new SuccessResponse<Department>(_unitOfWork.Department.Remove(department));
    }
}