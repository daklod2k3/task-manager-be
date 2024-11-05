using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using server.Context;
using server.Entities;
using server.Services;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationController : Controller
{
    SupabaseContext _context;
    private readonly IConfiguration _configuration;
    public NotificationController(SupabaseContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    

    //lấy mọi notifications
    [HttpGet]
    public ActionResult<IEnumerable<Notification>> Get()
    {
        return _context.Notifications.ToList();
        
    }


    //tạo notification mới
    [HttpPost]
    public ActionResult<Notification> Post(Notification notification)
    {
        _context.Notifications.Add(notification);
        _context.SaveChanges();

        return CreatedAtAction(nameof(Get), new { id = notification.Id }, notification);
    }


    // lấy tất cả notifications dựa theo token của user
    [HttpGet("{accessToken}")]
    public ActionResult<IEnumerable<Notification>> Get(string accessToken)
{
    // Check if the access token is provided in the header
    if (Request.Headers.TryGetValue("Authorization", out var authHeaderValue))
    {
        // Extract the token from the header
        var authHeader = authHeaderValue.FirstOrDefault();
        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            // Remove the "Bearer " prefix
            var token = authHeader.Substring(7);

            // Validate the access token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
                var jwtToken = (JwtSecurityToken)securityToken;
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    int userId = int.Parse(userIdClaim.Value);
                    return _context.Notifications.Where(n => n.UserId.Equals(userId)).ToList();
                }
                else
                {
                    return BadRequest("Invalid access token");
                }
            }
            catch (Exception)
            {
                return BadRequest("Invalid access token");
            }
        }
        else
        {
            return BadRequest("Access token not found in header");
        }
    }
    else
    {
        return BadRequest("Access token not found in header");
    }
}


//gửi entity lên server, server trả về entity read = true
[HttpPut]
public ActionResult<Notification> Put(Notification notification)
{
    var existingNotification = _context.Notifications.Find(notification.Id);

    if (existingNotification == null)
    {
        return NotFound();
    }


    existingNotification.Read = true; // Update the Read property to true

    _context.SaveChanges();


    return existingNotification;
}
}
