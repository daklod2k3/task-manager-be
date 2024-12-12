using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;

namespace server.Controllers.User;

[ApiController]
[Route("/user/[controller]")]
public class UserController: Controller
{

    private readonly IRepository<Profile> _repository;
    
    public UserController(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.Users;
    }
    
    [HttpGet]
    public ActionResult GetUser()
    {
        Guid userId = new Guid(AuthController.GetUserId(HttpContext));
        var user = _repository.Get(t =>
            t.Id == userId).FirstOrDefault();
       
        return new SuccessResponse<Profile>(user);
    }

    [HttpPut]
    public ActionResult UpdateUser(Profile user)
    {
        var id = AuthController.GetUserId(HttpContext);
        var tuser = GetUser(user.Id);
        if(tuser == default){
            return new ErrorResponse("User is not found");
        }
        if(tuser.Id != new Guid(id)){
            return new ErrorResponse("You can't change this");
        }
        if( tuser.Id == new Guid(id) && (tuser.RoleId != user.RoleId || 
        tuser.CreatedAt != user.CreatedAt)){
            return new ErrorResponse("You can't change this");
        }
        user.Role = null;
        var result = _repository.Update(user);
        _repository.Save();
        return new SuccessResponse<Profile>(result);
    }

    public Profile GetUser(Guid id){
        var iduser = new Guid(AuthController.GetUserId(HttpContext));
        var user = _repository.Get(t =>
            t.Id == iduser).FirstOrDefault(u => u.Id == id);
        return user;
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteUser(Guid id)
    {
        var user = GetUser(id);
        if(user == default){
            return new ErrorResponse("User is not found");
        }
        if(user.Id != new Guid(AuthController.GetUserId(HttpContext))){
            return new ErrorResponse("You can't delete this");
        }
        var result = _repository.Remove(user);
        _repository.Save();
        return new SuccessResponse<Profile>(result);
    }
}