using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
    [HttpGet]
    public ActionResult GetUserAuthorized()
    {
        var user_id = AuthController.GetUserId(HttpContext);

        return GetProfile(user_id);
    }

    [HttpGet("{user_id}")]
    public ActionResult GetProfile(string user_id)
    {
        if (user_id == null) 
            return new ErrorResponse("no user found");
        var user = _userService.GetProfile(new Guid(user_id));
        return new SuccessResponse<Profile>( user );
    }
}