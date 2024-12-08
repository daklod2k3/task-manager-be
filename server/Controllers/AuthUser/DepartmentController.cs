using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;

namespace server.Controllers.AuthUser;

[ApiController]
[Route("auth/user/[controller]")]
public class DepartmentController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Profile> Users;

    public DepartmentController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        Users = _unitOfWork.Users;
    }

    [HttpGet]
    public ActionResult GetDepartments()
    {
        var userId = new Guid(AuthController.GetUserId(HttpContext));
        var departmentList = _unitOfWork.Departments.Get(t =>
            t.DepartmentUsers.Any(du => du.UserId == userId));

        return new SuccessResponse<IEnumerable<Department>>(departmentList);
    }

    [HttpPost]
    public ActionResult CreateDepartment(Department department)
    {
        var id = AuthController.GetUserId(HttpContext);
        var d = _unitOfWork.Departments.Add(department);
        _unitOfWork.DepartmentUsers.Add(new DepartmentUser
            { DepartmentId = d.Id, UserId = new Guid(id), OwnerType = EDepartmentOwnerType.Owner });
        return new SuccessResponse<Department>(d);
    }

    [HttpPut]
    public ActionResult UpdateDepartment(Department department)
    {
        var id = AuthController.GetUserId(HttpContext);
        if (_unitOfWork.Departments.GetById(department.Id) == null) return new ErrorResponse("Department not found");
        if (!department.DepartmentUsers.Any(du =>
                du.UserId == new Guid(id) && du.OwnerType == EDepartmentOwnerType.Owner))
            return new ErrorResponse("You can't update this Department");
        return new SuccessResponse<Department>(_unitOfWork.Departments.Update(department));
    }

    [HttpPatch("{id}")]
    public ActionResult UpdateDepartment(long id, [FromBody] JsonPatchDocument<Department> patchDoc)
    {
        var iduser = AuthController.GetUserId(HttpContext);
        var department = _unitOfWork.Departments.GetById(id);
        if (_unitOfWork.Departments.GetById(department.Id) == null) return new ErrorResponse("Department not found");
        if (!department.DepartmentUsers.Any(du =>
                du.UserId == new Guid(iduser) && du.OwnerType == EDepartmentOwnerType.Owner))
            return new ErrorResponse("You can't update this Department");
        return new SuccessResponse<Department>(_unitOfWork.Departments.UpdatePatch(id, patchDoc));
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteTask(long id)
    {
        var iduser = AuthController.GetUserId(HttpContext);
        var department = _unitOfWork.Departments.GetById(id);
        if (_unitOfWork.Departments.GetById(department.Id) == null) return new ErrorResponse("Department not found");
        if (!department.DepartmentUsers.Any(du =>
                du.UserId == new Guid(iduser) && du.OwnerType == EDepartmentOwnerType.Owner))
            return new ErrorResponse("You can't update this Department");
        return new SuccessResponse<Department>(_unitOfWork.Departments.Remove(department));
    }
}