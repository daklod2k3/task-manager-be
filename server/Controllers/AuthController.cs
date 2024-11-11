using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supabase.Gotrue;
using Supabase.Gotrue.Exceptions;
using Client = Supabase.Client;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : Controller
{
    // private Client CreateStatelessClient()
    // {
    //     return new Client(new ClientOptions
    //     {
    //         Url = Environment.GetEnvironmentVariable("SUPABASE_URL") + "/auth/v1",
    //         Headers = new Dictionary<string, string>
    //         {
    //             { "apikey", Environment.GetEnvironmentVariable("SUPABASE_PUB_KEY") }
    //         },
    //     });
    // }
    private readonly Client _client;

    public AuthController(Client client)
    {
        _client = client;
    }

    public static string GetUserId(HttpContext context)
    {
        return context.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }


    // create user
    // [HttpPost()]
    // public string register([FromBody] RegisterModel registerModel)
    // {
    //     var options = new SignUpOptions();
    //     options.Data = new Dictionary<string, object>()
    //     {
    //         { "name", registerModel.name }
    //     };
    //      var result = SupabaseClient.AuthClient.SignUp(registerModel.email, registerModel.password, options);
    //      // result.Start();
    //      result.Wait();
    //      
    //      // Debug.WriteLine(result.Result);
    //     return result.Result.ToString();
    // }

    // create user
    [AllowAnonymous]
    [HttpPost]
    [Route("register")]
    public IActionResult register([FromBody] RegisterBody registerModel)
    {
        var options = new SignUpOptions();
        options.Data = new Dictionary<string, object>
        {
            { "name", registerModel.name }
        };
        try
        {
            var result = _client.Auth.SignUp(registerModel.email, registerModel.password, options);
            return Ok(result.Result);
        }
        catch (AggregateException e)
        {
            return BadRequest(((GotrueException)e.InnerException).Message);
        }
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public IActionResult login([FromBody] LoginBody registerModel)
    {
        try
        {
            var result = _client.Auth.SignIn(registerModel.email, registerModel.password);
            return Ok(result.Result);
        }
        catch (AggregateException e)
        {
            var gotrueException = e.InnerException as GotrueException;
            return StatusCode(gotrueException.StatusCode, gotrueException.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }


    public class LoginBody
    {
        public string email { get; set; }
        public string password { get; set; }
    }

    public class RegisterBody : LoginBody
    {
        public string name { get; set; }
    }
}