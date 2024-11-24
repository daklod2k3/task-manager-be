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
        return _unitOfWork.Channel.GetAll();
    }

    public Channel DeleteChannel(long id)
    {
        var Channel = _unitOfWork.Channel.Get(x => x.Id == id);
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
        var noti = _unitOfWork.Channel.Get(x => x.Id == id);
        if (noti == null) throw new Exception("Channel not found");

        Channel.ApplyTo(noti);

        _unitOfWork.Save();

        return noti;
    }


    public Channel? GetChannelById(long id)
    {
        if (0L == id) return null;
        var result = _unitOfWork.Channel.Get(t => t.Id == id);
        return result;
    }

    public IEnumerable<Channel> GetChannelByFilter(Expression<Func<Channel, bool>> filter)
    {
        return _unitOfWork.Channel.GetAll(filter);
    }

    
}