using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using System.Linq.Expressions;

namespace server.Interfaces;
    public interface IChannelUserService
    {
        public ChannelUser CreateChannelUser(ChannelUser department);
        public ChannelUser UpdateChannelUser(ChannelUser department);
        public ChannelUser DeleteChannelUser(long idChannelUser);
        public ChannelUser UpdateChannelUserPatch(long id, [FromBody] JsonPatchDocument<ChannelUser> patchDoc);
        public ChannelUser GetChannelUser(long id);
        public IEnumerable<ChannelUser> GetChannelUserByFilter(Expression<Func<ChannelUser, bool>> compositeFilterExpression);
    
    }
