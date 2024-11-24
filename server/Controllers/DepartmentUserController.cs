
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Services;
using server.Interfaces;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentUserController : ControllerBase
    {
        private readonly IDepartmentUserService _service;

        public DepartmentUserController(IDepartmentUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DepartmentUser>> Get()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<DepartmentUser> GetById(long id)
        {
            return Ok(_service.GetById(id));
        }

        [HttpPost]
        public ActionResult<DepartmentUser> Create(DepartmentUser departmentUser)
        {
            var created = _service.Create(departmentUser);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public ActionResult<DepartmentUser> Update(long id, DepartmentUser departmentUser)
        {
            // Tìm DepartmentUser trước khi cập nhật
            var existingDepartmentUser = _service.GetById(id);
            if (existingDepartmentUser == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu không tìm thấy
            }

            // Cập nhật thông tin
            departmentUser.Id = id; // Đảm bảo ID là id từ URL
            var updated = _service.Update(departmentUser); // Gọi phương thức Update từ service

            return Ok(updated);
        }


        [HttpPatch("{id}")]
        public ActionResult<DepartmentUser> UpdatePatch(long id, [FromBody] JsonPatchDocument<DepartmentUser> patchDoc)
        {
            // Áp dụng patch
            var updated = _service.UpdatePatch(id, patchDoc);

            // Kiểm tra nếu không cập nhật được, trả về lỗi 404
            if (updated == null)
            {
                return NotFound();
            }

            return Ok(updated);
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _service.Delete(id);
            return NoContent();

        }
    }
}