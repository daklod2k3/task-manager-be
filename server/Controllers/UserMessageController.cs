using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using server.Entities;
using server.Helpers;
using server.Interfaces;
using server.Services;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserMessageController : Controller
{
    private readonly IUserMessageService _usermessageService;

    public UserMessageController(IUserMessageService usermessageService)
    {
        _usermessageService = usermessageService;
    }

    [HttpPost]
    public ActionResult CreateUserMessage(UserMessage usermessage)
    {
        try
        {
            return new SuccessResponse<UserMessage>(_usermessageService.CreatUserMessage(usermessage));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("UserMessage is not create");
        }
    }

    [HttpPut]
    public ActionResult UpdateUserMessage(UserMessage usermessage)
    {
        try
        {
            return new SuccessResponse<UserMessage>(_usermessageService.UpdateUserMessage(usermessage));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("UserMessage is not update");
        }
    }

    [HttpPatch("{id}")]
    public ActionResult UpdateUserMessagePatch(long id, [FromBody] JsonPatchDocument<UserMessage> patchDoc)
    {
        try
        {
            return new SuccessResponse<UserMessage>(_usermessageService.UpdateUserMessagePatch(id, patchDoc));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("usermessage is not update");
        }
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteUserMessage(long id)
    {
        try
        {
            return new SuccessResponse<UserMessage>(_usermessageService.DeleteUserMessage(id));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("usermessage is not delete");
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public ActionResult<IEnumerable<UserMessage>> GetUserMessageByFilter(string filterString)
    {
        var filterResult = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString))
            filterResult = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        var compositeFilterExpression = CompositeFilter<UserMessage>.ApplyFilter(filterResult);

        var DepartmetnList = _usermessageService.GetUserMessageByFilter(compositeFilterExpression);
        return new SuccessResponse<IEnumerable<UserMessage>>(DepartmetnList);
    }

    [HttpGet]
    public ActionResult<IEnumerable<UserMessage>> Get(string? filter)
    {
        return GetUserMessageByFilter(filter);
    }

    [HttpGet]
    [Route("{usermessageId}")]
    public ActionResult<IEnumerable<UserMessage>> GetUserMessageById(long usermessageId)
    {
        try
        {
            return new SuccessResponse<UserMessage>(_usermessageService.GetUserMessage(usermessageId));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("usermessage is not get");
        }
    }
}