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
public class ChannelMessageController : Controller
{
    private readonly IChannelMessageService _channelmessageService;

    public ChannelMessageController(IChannelMessageService channelmessageService)
    {
        _channelmessageService = channelmessageService;
    }

    [HttpPost]
    public ActionResult CreateChannelMessage(ChannelMessage channelmessage)
    {
        try
        {
            return new SuccessResponse<ChannelMessage>(_channelmessageService.CreateChannelMessage(channelmessage));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("ChannelMessage is not create");
        }
    }

    [HttpPut]
    public ActionResult UpdateChannelMessage(ChannelMessage channelmessage)
    {
        try
        {
            return new SuccessResponse<ChannelMessage>(_channelmessageService.UpdateChannelMessage(channelmessage));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("ChannelMessage is not update");
        }
    }

    [HttpPatch("{id}")]
    public ActionResult UpdateChannelMessagePatch(long id, [FromBody] JsonPatchDocument<ChannelMessage> patchDoc)
    {
        try
        {
            return new SuccessResponse<ChannelMessage>(_channelmessageService.PatchChannelMessage(id, patchDoc));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("channelmessage is not update");
        }
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteChannelMessage(long id)
    {
        try
        {
            return new SuccessResponse<ChannelMessage>(_channelmessageService.DeleteChannelMessage(id));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("channelmessage is not delete");
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public ActionResult<IEnumerable<ChannelMessage>> GetChannelMessageByFilter(string filterString)
    {
        var filterResult = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString))
            filterResult = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        var compositeFilterExpression = CompositeFilter<ChannelMessage>.ApplyFilter(filterResult);

        var DepartmetnList = _channelmessageService.GetChannelMessageByFilter(compositeFilterExpression);
        return new SuccessResponse<IEnumerable<ChannelMessage>>(DepartmetnList);
    }

    [HttpGet]
    public ActionResult<IEnumerable<ChannelMessage>> Get(string? filter)
    {
        return GetChannelMessageByFilter(filter);
    }

    [HttpGet]
    [Route("{channelmessageId}")]
    public ActionResult<IEnumerable<ChannelMessage>> GetChannelMessageById(long channelmessageId)
    {
        try
        {
            return new SuccessResponse<ChannelMessage>(_channelmessageService.GetChannelMessage(channelmessageId));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("channelmessage is not get");
        }
    }
}