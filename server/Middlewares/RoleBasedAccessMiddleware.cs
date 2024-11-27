using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net;
using server.Controllers;
using Supabase;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using server.Entities;
using server.Interfaces;
using server.Context;
using server.Repository;

public class RoleBasedAccessMiddleware
{
    private readonly RequestDelegate _next;
    private IUnitOfWork _unitOfWork;

    public RoleBasedAccessMiddleware(RequestDelegate next)
    {
        _next = next;
        
    }

    public async Task InvokeAsync(HttpContext context,SupabaseContext db)
    {
        _unitOfWork = new UnitOfWork(db);
        // var claims = context.User.Claims.Select(c => new { c.Type, c.Value });
        // foreach (var claim in claims)
        //     {
        //         Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
        //     }
        if (!context.User.Identity.IsAuthenticated && 
            context.Request.Path != "/auth/login" && 
            context.Request.Path != "/auth/register")
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorResponse("Unauthorized")
                { Status = HttpStatusCode.Unauthorized }));
            return;
        }

        var user_id = context.User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(user_id))
        {
            user_id = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;  // Fallback to 'nameidentifier'
        }
        //Console.WriteLine(_unitOfWork.User);
        long? role_id= _unitOfWork.User.GetById(user_id,"Role")?.RoleId;
        
        

        var role_name = _unitOfWork.Role.GetById(role_id).Name;
        Console.WriteLine(role_name);
        // Define the roles required for this request (you can get this from route data, metadata, etc.)
        var requiredRoles = GetRequiredRolesForRequest(context); // Custom logic to get required roles
        //Check if the user has any of the required roles
        if (!requiredRoles.Any(s => s == role_name))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorResponse("Unauthorized")
                { Status = HttpStatusCode.Unauthorized }));
            return;
        }

        var perm = _unitOfWork.Role.GetById(role_id).Permissions;
        var endpoint = context.Request.Path.Value;
        var method = context.Request.Method;
        Console.WriteLine($"Endpoint: {endpoint}, Method: {method}");
        Console.WriteLine($"Permissions: {string.Join(", ", perm)}");
        
        // if (!perm.Any(s => s.endpoint == endpoint && s.method == method))
        // {
        //     context.Response.StatusCode = StatusCodes.Status403Forbidden;
        //     await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorResponse("Unauthorized")
        //         { Status = HttpStatusCode.Unauthorized }));
        //     return;
        // }

        // If everything is fine, continue processing the request
        await _next(context);
    }

    private List<string?> GetRequiredRolesForRequest(HttpContext context)
    {
        var roles = _unitOfWork.Role.Get();
        
        return roles.Select(r => r.Name).ToList();
    }
}