using Microsoft.AspNetCore.Mvc;
using server.Context;
using server.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using server.Helpers;
using server.Interfaces;
using System.Linq.Expressions;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class ChannelController : Controller
{
    private readonly IChannelService _channelService;

    public ChannelController(IChannelService channelService)
    {
        _channelService = channelService;
    }

    [HttpPost]
    public ActionResult CreateChannel(Channel channel)
    {
        var id = AuthController.GetUserId(HttpContext);
        channel.CreatedBy = new Guid(id);
        try
        {
            return new SuccessResponse<Channel>(_channelService.CreateChannel(channel));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Channel is not create");
        }
    }

    [HttpPut]
    public ActionResult UpdateChannel(Channel channel)
    {
        try
        {
            return new SuccessResponse<Channel>(_channelService.UpdateChannel(channel));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Channel is not update");
        }
    }

    [HttpPatch("{id}")]
    public ActionResult PatchChannel(long id, [FromBody] JsonPatchDocument<Channel> patchDoc)
    {
        try
        {
            return new SuccessResponse<Channel>(_channelService.PatchChannel(id, patchDoc));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Channel is not update");
        }
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteChannel(long id)
    {
        try
        {
            return new SuccessResponse<Channel>(_channelService.DeleteChannel(id));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Channel is not delete");
        }
    }


    [HttpGet]
    [Route("{channelId}")]
    public ActionResult<IEnumerable<Channel>> GetChannelById(long Id)
    {
        var id = AuthController.GetUserId(HttpContext);
        try
        {
            var channel = _channelService.GetChannelById(Id);
            if (channel == null)
            {
                return new ErrorResponse("Channel is not found");
            }
            return new SuccessResponse<Channel>(channel);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new ErrorResponse("Channel is not found");
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public ActionResult<IEnumerable<Channel>> GetChannelByFilter(string filterString)
    {
        var filterResult = new ClientFilter();
        if (!string.IsNullOrEmpty(filterString))
            filterResult = JsonConvert.DeserializeObject<ClientFilter>(filterString);
        var compositeFilterExpression = CompositeFilter<Channel>.ApplyFilter(filterResult);

        var taskList = _channelService.GetChannelByFilter(compositeFilterExpression);
        return new SuccessResponse<IEnumerable<Channel>>(taskList);
    }
}