using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace server.Controllers;

[DefaultStatusCode(400)]
public class ErrorResponse(string message = "Unexpected error") : StatusCodeResult(400)
{
    [JsonConverter(typeof(JsonNumberEnumConverter<HttpStatusCode>))]
    public HttpStatusCode Status { get; set; } = HttpStatusCode.BadRequest;

    public string Message { get; set; } = message;
}

[DefaultStatusCode(200)]
public class SuccessResponse<T>() : StatusCodeResult(200)
{
    [JsonConverter(typeof(JsonNumberEnumConverter<HttpStatusCode>))]
    public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;

    public string? Message { get; set; }
    public IEnumerable<T> Data { get; set; }
}