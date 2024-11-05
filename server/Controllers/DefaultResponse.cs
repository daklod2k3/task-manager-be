using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace server.Controllers;

[DefaultStatusCode(400)]
public class ErrorResponse : ActionResult
{
    [JsonConverter(typeof(JsonNumberEnumConverter<HttpStatusCode>))]
    public HttpStatusCode Status { get; set; } = HttpStatusCode.BadRequest;

    public string Message { get; set; }

    public ErrorResponse()
    {
        Message = "Unexpected error";
    }

    public ErrorResponse(string message)
    {
        Message = message;
    }
    
    public override async Task ExecuteResultAsync(ActionContext context)
    {
        var jsonResult = new JsonResult(this);
        await jsonResult.ExecuteResultAsync(context);
    }
}

public sealed class SuccessResponse<T>(IEnumerable<T> data) : ActionResult
{
    [JsonConverter(typeof(JsonNumberEnumConverter<HttpStatusCode>))]
    public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;

    public string? Message { get; set; } = "Success";
    public IEnumerable<T> Data { get; set; } = data;


    public override async Task ExecuteResultAsync(ActionContext context)
    {
        var jsonResult = new JsonResult(this);
        await jsonResult.ExecuteResultAsync(context);
    }
}