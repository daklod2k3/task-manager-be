using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using System.Linq.Expressions;

namespace server.Interfaces;
    public interface IChannelMessageService
    {
        public ChannelMessage CreateChannelMessage(ChannelMessage channelmessage);
        public ChannelMessage UpdateChannelMessage(ChannelMessage channelmessage);
        public ChannelMessage DeleteChannelMessage(long idChannelMessage);
        public ChannelMessage PatchChannelMessage(long id, [FromBody] JsonPatchDocument<ChannelMessage> patchDoc);
        public ChannelMessage GetChannelMessage(long id);
        public IEnumerable<ChannelMessage> GetChannelMessageByFilter(Expression<Func<ChannelMessage, bool>> compositeFilterExpression);
    
    }
