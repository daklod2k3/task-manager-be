using server.Entities;
using System.Linq.Expressions;
using Microsoft.AspNetCore.JsonPatch;
namespace server.Interfaces;
using Microsoft.AspNetCore.Mvc;

public interface IChannelService
{
    IEnumerable<Channel> GetAllChannels();

    Channel CreateChannel(Channel channel);

    Channel DeleteChannel(long id);
    Channel UpdateChannel(Channel channel);
    Channel PatchChannel(long id, [FromBody] JsonPatchDocument<Channel> channel);
    public Channel? GetChannelById(long id);
    public IEnumerable<Channel> GetChannelByFilter(Expression<Func<Channel, bool>>? compositeFilterExpression);



}