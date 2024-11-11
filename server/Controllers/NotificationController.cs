using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using server.Context;
using server.Entities;
using server.Interfaces;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationController : Controller
{
    private readonly INotificationService _notificationService;
    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    

    //lấy mọi notifications
    [HttpGet]
    public ActionResult<IEnumerable<Notification>> Get()
    {
        return _notificationService.GetAllNotifications().ToList();
        
    }


    //tạo notification mới
    [HttpPost]
    public ActionResult<Notification> CreateNotification(Notification notification)
    {
        try
        {
            return new SuccessResponse<Notification>(_notificationService.CreateNotification(notification));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Notification failed to create");
        }
    }


//     // lấy tất cả notifications dựa theo token của user
//     [HttpGet("{accessToken}")]
//     public ActionResult<IEnumerable<Notification>> Get(string accessToken)
// {
//     // Check if the access token is provided in the header
//     if (Request.Headers.TryGetValue("Authorization", out var authHeaderValue))
//     {
//         // Extract the token from the header
//         var authHeader = authHeaderValue.FirstOrDefault();
//         if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
//         {
//             // Remove the "Bearer " prefix
//             var token = authHeader.Substring(7);

//             // Validate the access token
//             var tokenHandler = new JwtSecurityTokenHandler();
//             var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
//             try
//             {
//                 var tokenValidationParameters = new TokenValidationParameters
//                 {
//                     ValidateIssuerSigningKey = true,
//                     IssuerSigningKey = new SymmetricSecurityKey(key),
//                     ValidateIssuer = false,
//                     ValidateAudience = false,
//                     ClockSkew = TimeSpan.Zero
//                 };

//                 var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
//                 var jwtToken = (JwtSecurityToken)securityToken;
//                 var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

//                 if (userIdClaim != null)
//                 {
//                     int userId = int.Parse(userIdClaim.Value);
//                     return _context.Notifications.Where(n => n.UserId.Equals(userId)).ToList();
//                 }
//                 else
//                 {
//                     return BadRequest("Invalid access token");
//                 }
//             }
//             catch (Exception)
//             {
//                 return BadRequest("Invalid access token");
//             }
//         }
//         else
//         {
//             return BadRequest("Access token not found in header");
//         }
//     }
//     else
//     {
//         return BadRequest("Access token not found in header");
//     }
// }


//gửi entity lên server, server trả về entity read = true
[HttpPut]
public ActionResult<Notification> UpdateNotification(Notification notification)
{
    try
        {
            return new SuccessResponse<Notification>(_notificationService.UpdateNotification(notification));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Notification did not update");
        }
}

[HttpDelete]
    public ActionResult DeleteNotification(long id)
    {
        try
        {
            return new SuccessResponse<Notification>(_notificationService.DeleteNotification(id));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Notification did not delete");
        }
    }
}
