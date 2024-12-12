using System.Net;
using Microsoft.AspNetCore.Mvc;
using server.Context;
using server.Entities;
using server.Interfaces;
using Client = Supabase.Client;

namespace server.Controllers.AuthUser;

[ApiController]
[Route("auth/user")]
public class ProfileController : Controller
{
    private readonly Client _client;
    private readonly IUserRepository _users;

    public ProfileController(SupabaseContext context, IUnitOfWork unitOfWork)
    {
        _users = unitOfWork.Users;
    }

    // GET
    [HttpGet]
    public ActionResult GetUserAuthorized([FromQuery] string? includes = "")
    {
        var user_id = AuthController.GetUserId(HttpContext);

        return GetProfile(user_id, includes);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public ActionResult GetProfile(string user_id, string? includes = "")
    {
        if (user_id == null)
            return new ErrorResponse("no user found");
        var user = _users.GetById(user_id, includes);
        return new SuccessResponse<Profile>(user);
    }

    [HttpPut]
    public ActionResult Update(Profile body)
    {
        var user_id = new Guid(AuthController.GetUserId(HttpContext));
        if (body.Id != user_id) return new ErrorResponse("Permission denied") { Status = HttpStatusCode.Forbidden };
        var entity = _users.Update(body);
        _users.Save();
        return new SuccessResponse<Profile>(entity);
    }
}