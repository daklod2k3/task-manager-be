// using server.Entities;
// using server.Interfaces;
// using Microsoft.AspNetCore.JsonPatch;
// using System.Linq;
//
// namespace server.Services
// {
//     public class ChannelService : IChannelService
//     {
//         private readonly IChannel _repository;
//
//         public ChannelService(IChannel repository)
//         {
//             _repository = repository;
//         }
//
//         public IEnumerable<Channel> GetAll(Guid userId)
//         {
//             // Lọc các Channel mà user hiện tại tham gia (có thể là qua ChannelUser)
//             return _repository.GetAll(x => x.CreatedBy == userId);
//         }
//
//         public Channel GetById(long id)
//         {
//             return _repository.Get(x => x.Id == id) ?? throw new KeyNotFoundException("Channel not found");
//         }
//
//         public Channel Create(Channel channel)
//         {
//             channel.CreatedAt = DateTime.UtcNow;
//             channel.CreatedBy = Guid.NewGuid(); // set user hiện tại là creator
//             var created = _repository.Add(channel);
//             _repository.Save();
//             return created;
//         }
//
//         public Channel Update(Channel channel)
//         {
//             return _repository.Update(channel);
//         }
//
//         public Channel UpdatePatch(long id, JsonPatchDocument<Channel> patch)
//         {
//             var channel = _repository.GetById(id.ToString());
//             if (channel == null) throw new KeyNotFoundException("Channel not found");
//             patch.ApplyTo(channel);
//             _repository.Update(channel);
//             _repository.Save();
//             return channel;
//         }
//
//         public void Delete(long id)
//         {
//             var channel = _repository.GetById(id.ToString());
//             if (channel != null)
//             {
//                 _repository.Remove(channel);
//                 _repository.Save();
//             }
//         }
//     }
// }

