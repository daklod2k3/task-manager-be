using Microsoft.AspNetCore.Mvc;
using server.Context;
using server.Entities;
using server.Interfaces;
using server.Repository;
using Client = Supabase.Client;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    private readonly Client _client;
    private readonly IUnitOfWork _unitOfWork;

    public UserController(SupabaseContext context)
    {
        _unitOfWork = new UnitOfWork(context);
    }

    // GET
    [HttpGet]
    public ActionResult GetUserAuthorized([FromQuery] string? includes = "")
    {
        var user_id = AuthController.GetUserId(HttpContext);

        return GetProfile(user_id, includes);
    }

    [HttpGet("{user_id}")]
    public ActionResult GetProfile(string user_id, string? includes = "")
    {
        if (user_id == null)
            return new ErrorResponse("no user found");
        var user = _unitOfWork.User.GetById(user_id, includes);
        return new SuccessResponse<Profile>(user);
    }
}