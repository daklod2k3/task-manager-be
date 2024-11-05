using System.Net;
using System.Text.Json;
using server.Controllers;
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
        if (context.Request.Cookies.ContainsKey("sb-ndoyladxdcpftovoalas-auth-token"))
            token = context.Request.Cookies["sb-ndoyladxdcpftovoalas-auth-token"].Replace("base64-", "");
        Console.WriteLine($"Token: {token}");
        if (string.IsNullOrEmpty(token))
        {
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorResponse("Unauthorized")
                { Status = HttpStatusCode.Unauthorized }));
            return;
        }

        var user = await _client.Auth.GetUser(token);
        if (user == null)
        {
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorResponse("Unauthorized")
                { Status = HttpStatusCode.Unauthorized }));
            return;
        }

        context.Items["user_id"] = user.Id;
        // Call the next middleware in the pipeline

        await _next(context);
    }
}