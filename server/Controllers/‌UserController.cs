using Microsoft.AspNetCore.Mvc;
using server.Context;
using server.Entities;
using server.Services;
using Supabase.Gotrue;
using Client = Supabase.Client;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    
    private readonly SupabaseContext _context;
    private readonly Client _client;
    private readonly UserService _userService;

    public UserController(SupabaseContext context, Client client, UserService userService)
    {
        _context = context;
        _client = client;
        _userService = userService;
    }
    
    // GET
    [HttpGet]
    public ActionResult Get()
    {
        String user_id = HttpContext.Items["user_id"] as String;
        if (user_id == null) return new NotFoundResult();
        var user = _userService.GetProfile(new Guid(user_id));
        return new SuccessResponse<Profile>(){Data = new []{ user }};
        
    }
    
}