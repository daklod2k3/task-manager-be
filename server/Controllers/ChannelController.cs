using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;
using System.Collections.Generic;

namespace server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ChannelController : ControllerBase
    {
        private readonly IChannelService _service;

        public ChannelController(IChannelService service)
        {
            _service = service;
        }

        // Lấy danh sách Channel của User hiện tại
        [HttpGet]
        public ActionResult<IEnumerable<Channel>> Get()
        {
            var userId = Guid.NewGuid(); // Thay thế bằng userId từ authentication context
            var channels = _service.GetAll(userId);
            return Ok(channels);
        }

        // Lấy chi tiết Channel của id cung cấp
        [HttpGet("{id}")]
        public ActionResult<Channel> GetById(long id)
        {
            var channel = _service.GetById(id);
            return Ok(channel);
        }

        // Tạo Channel mới
        [HttpPost]
        public ActionResult<Channel> Create([FromBody] Channel channel)
        {
            var createdChannel = _service.Create(channel);
            return CreatedAtAction(nameof(GetById), new { id = createdChannel.Id }, createdChannel);
        }

        // Cập nhật Channel
        [HttpPut("{id}")]
        public ActionResult<Channel> Update(long id, [FromBody] Channel channel)
        {
            channel.Id = id; // Đảm bảo id được giữ nguyên
            var updatedChannel = _service.Update(channel);
            return Ok(updatedChannel);
        }

        // Cập nhật Channel với Json Patch
        [HttpPatch("{id}")]
        public ActionResult<Channel> UpdatePatch(long id, [FromBody] JsonPatchDocument<Channel> patchDoc)
        {
            var updatedChannel = _service.UpdatePatch(id, patchDoc);
            return Ok(updatedChannel);
        }

        // Xoá Channel
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _service.Delete(id);
            return NoContent();
        }
    }
}
