using Microsoft.AspNetCore.Mvc;
using server.Context;
using server.Entities;
using Supabase.Gotrue;
using Client = Supabase.Client;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    
    private readonly SupabaseContext _context;
    private readonly Client _client;

    public UserController(SupabaseContext context, Client client)
    {
        _context = context;
        _client = client;
    }
    
    // GET
    [HttpGet]
    public ActionResult Get()
    {
        var user = HttpContext.Items["user"] as User;
        return Ok(HttpContext.Items["User"]);
        
    }
    
}