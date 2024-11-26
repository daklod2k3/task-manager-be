using System.Linq.Expressions;
using LinqKit;
using server.Entities;
using server.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace server.Services;

public class ChannelService : IChannelService
{
    private readonly IUnitOfWork _unitOfWork;

    public ChannelService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Channel CreateChannel(Channel Channel)
    {
        var result = _unitOfWork.Channel.Add(Channel);
        _unitOfWork.Save();
        return result;
    }

    public IEnumerable<Channel> GetAllChannels()
    {
        CreateChannel(new Channel());
        return _unitOfWork.Channel.Get();
    }

    public Channel DeleteChannel(long id)
    {
        var Channel = _unitOfWork.Channel.GetById(id);
        var result = _unitOfWork.Channel.Remove(Channel);
        _unitOfWork.Save();
        return result;
    }

    public Channel UpdateChannel(Channel Channel)
    {
        var result = _unitOfWork.Channel.Update(Channel);
        return result;
    }

    public Channel PatchChannel(long id,[FromBody] JsonPatchDocument<Channel> Channel)
    {
        var channel = _unitOfWork.Channel.GetById(id);
        if (channel == null) throw new Exception("Channel not found");

        Channel.ApplyTo(channel);

        _unitOfWork.Save();

        return channel;
    }


    public Channel GetChannelById(long id)
    {
        return _unitOfWork.Channel.GetById(id);
    }

    public IEnumerable<Channel> GetChannelByFilter(Expression<Func<Channel, bool>> filter)
    {
        return _unitOfWork.Channel.Get(filter, includeProperties: "ChannelMessages,ChannelUsers");
    }

    
}