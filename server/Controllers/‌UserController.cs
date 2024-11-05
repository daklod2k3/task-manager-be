using Microsoft.AspNetCore.Mvc;
using server.Context;
using server.Entities;
using server.Services;
using Client = Supabase.Client;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    private readonly Client _client;

    private readonly SupabaseContext _context;
    private readonly UserService _userService;

    public UserController(SupabaseContext context, Client client, UserService userService)
    {
        _context = context;
        _client = client;
        _userService = userService;
    }

    // GET
    public ActionResult GetAuthUser()
    {
        var user_id = HttpContext.Items["user_id"] as string;
        if (user_id == null) return new NotFoundResult();
        var user = _userService.GetProfile(new Guid(user_id));
        return new SuccessResponse<Profile>(new List<Profile> { user });
    }

    [HttpGet("{user_id}")]
    public ActionResult Get(string user_id)
    {
        if (user_id == null) return GetAuthUser();
        var user = _userService.GetProfile(new Guid(user_id));
        return new SuccessResponse<Profile>(new List<Profile> { user });
    }
}