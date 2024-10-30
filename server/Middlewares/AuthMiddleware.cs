using Supabase;

namespace server.Middlewares;

public class AuthMiddleware
{
    private readonly Client _client;
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next, Client client)
    {
        _next = next;
        _client = client;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/auth"))
        {
            await _next(context);
            return;
        }

        var token = "";
        // Your custom authentication logic here
        if (context.Request.Headers.ContainsKey("Authorization"))
            token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (context.Request.Cookies.ContainsKey("acessToken"))
            token = context.Request.Cookies["acessToken"];
        if (string.IsNullOrEmpty(token))
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Authorization token missing");
            return;
        }

        try
        {
            var user = await _client.Auth.GetUser(token);
            if (user == null) throw new UnauthorizedAccessException();
            context.Items["user_id"] = user.Id;
            // Call the next middleware in the pipeline
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Authorization failed");
            return;
        }

        await _next(context);
    }
}