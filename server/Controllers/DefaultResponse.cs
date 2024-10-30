using System.Net;
using System.Text.Json.Serialization;

namespace server.Controllers;

public class ErrorResponse
{
    public HttpStatusCode Status { get; set; }
    public string Message { get; set; }
}

public class SuccessResponse<T>
{

    [JsonConverter(typeof(JsonNumberEnumConverter<HttpStatusCode>))]
    public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;

    public string? Message { get; set; }
    public IEnumerable<T> Data { get; set; }
}