using server.Entities;
using Microsoft.AspNetCore.JsonPatch;
namespace server.Interfaces
{
    public interface IChannelService
    {
        IEnumerable<Channel> GetAll(Guid userId);
        Channel GetById(long id);
        Channel Create(Channel channel);
        Channel Update(Channel channel);
        Channel UpdatePatch(long id, JsonPatchDocument<Channel> patch);
        void Delete(long id);
    }
}
