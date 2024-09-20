using System.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using server.Helpers;
using Supabase.Gotrue;
using Supabase.Gotrue.Exceptions;
using Supabase.Gotrue.Interfaces;
using static Supabase.Gotrue.StatelessClient;


namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : Controller
{


    public class LoginBody
    {
        public string email { get; set; }
        public string password { get; set; }
    }
    
    public class RegisterBody: LoginBody
    {
        
        public string name { get; set; }
    }
    
    public AuthController()
    {
    }

    private Client CreateStatelessClient()
    {
        return new Supabase.Gotrue.Client(new ClientOptions
        {
            Url = Environment.GetEnvironmentVariable("SUPABASE_URL") + "/auth/v1",
            Headers = new Dictionary<string, string>
            {
                { "apikey", Environment.GetEnvironmentVariable("SUPABASE_PUB_KEY") },
            }
        });
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
    [HttpPost()]
    [Route("register")]
    public string register([FromBody] RegisterBody registerModel)
    {
        var options = new SignUpOptions();
        options.Data = new Dictionary<string, object>()
        {
            { "name", registerModel.name }
        };
        var client = CreateStatelessClient();
        try
        {
            var result = client.SignUp(registerModel.email, registerModel.password, options);
            return result.Result.User.ToString();

        }
        catch (AggregateException e)
        {
            return ((GotrueException)e.InnerException).StatusCode.ToString();
        }
    }
    
    [HttpPost()]
    [Route("login")]
    public IActionResult login([FromBody] LoginBody registerModel)
    {
        var client = CreateStatelessClient();
        try
        {
            var result = client.SignIn(registerModel.email, registerModel.password);
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

    

    
    
}