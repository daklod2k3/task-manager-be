using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace server.Controllers;

[DefaultStatusCode(400)]
public class ErrorResponse : ActionResult
{
    public ErrorResponse()
    {
        Error = "Unexpected error";
    }

    public ErrorResponse(object error)
    {
        Error = error;
    }

    [JsonConverter(typeof(JsonNumberEnumConverter<HttpStatusCode>))]
    public HttpStatusCode Status { get; set; } = HttpStatusCode.BadRequest;

    public object Error { get; set; }

    public override async Task ExecuteResultAsync(ActionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)Status;
        var jsonResult = new JsonResult(this);
        await jsonResult.ExecuteResultAsync(context);
    }
}

public class SuccessResponse<T>(T data) : ActionResult
{
    [JsonConverter(typeof(JsonNumberEnumConverter<HttpStatusCode>))]
    public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;

    public string? Message { get; set; } = "Success";
    public T Data { get; set; } = data;


    public override async Task ExecuteResultAsync(ActionContext context)
    {
        var jsonResult = new JsonResult(this);
        await jsonResult.ExecuteResultAsync(context);
    }
}

// public void SuccessResponse(object data)
// {
//     return new SuccessResponse<typeo>(data)
// }
