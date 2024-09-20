using Supabase;

namespace server.Middlewares;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Client _client;

    public AuthMiddleware(RequestDelegate next, Client client)
    {
        _next = next;
        _client = client;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Your custom authentication logic here
        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Authorization header missing");
            return;
        }
        
        string token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        
        var user = await _client.Auth.GetUser(token);
        if (user == null)
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Authorization header missing");
            return;
        }
        
        context.Items["User"] = user;
        // Call the next middleware in the pipeline
        await _next(context);
    }
}