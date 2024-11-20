using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;
using Microsoft.AspNetCore.JsonPatch;

namespace server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskUserController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly ITaskUserService _taskUserService;
        public TaskUserController(ITaskService taskService, ITaskUserService taskUserService)
        {
            _taskService = taskService;
            _taskUserService = taskUserService;
        }

        //phuong thuc get
        [HttpGet]
        public IActionResult GetAllTaskUsers()
        {
            var taskUsers = _taskUserService.GetAllTaskUsers();
            return Ok(taskUsers);
        }
        //phuong thuc get theo id
        [HttpGet("{id}")]
        public IActionResult GetTaskUserById(int id)
        {
            try
            {
                var taskUser = _taskUserService.GetTaskUserById(id);
                if (taskUser == null)
                {
                    return NotFound(new { message = "TaskUser không tồn tại" });
                }
                return Ok(taskUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể lấy thông tin TaskUser" });
            }
        }
         // Tạo TaskUser mới
        [HttpPost]
        public IActionResult CreateTaskUser([FromBody] TaskUser taskUser)
        {
            try
            {
                if (taskUser == null)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ" });
                }

                _taskUserService.CreateTaskUser(taskUser);
                return CreatedAtAction(nameof(GetTaskUserById), new { id = taskUser.Id }, taskUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể tạo TaskUser" });
            }
        }

        // Cập nhật thông tin TaskUser toàn bộ
        [HttpPut("{id}")]
        public IActionResult UpdateTaskUser(int id, [FromBody] TaskUser taskUser)
        {
            try
            {
                if (taskUser == null || taskUser.Id != id)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ" });
                }

                bool result = _taskUserService.UpdateTaskUser(id,taskUser);
                if (!result)
                {
                    return NotFound(new { message = "TaskUser không tồn tại" });
                }

                return Ok(new { message = "Cập nhật TaskUser thành công" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật TaskUser" });
            }
        }

        // Cập nhật thông tin TaskUser với JSON Patch
        [HttpPatch("{id}")]
        public IActionResult PatchTaskUser(int id, [FromBody] JsonPatchDocument<TaskUser> patchDoc)
        {
            try
            {
                if (patchDoc == null)
                {
                    return BadRequest(new { message = "Patch document không hợp lệ" });
                }

                var taskUser = _taskUserService.GetTaskUserById(id);
                if (taskUser == null)
                {
                    return NotFound(new { message = "TaskUser không tồn tại" });
                }

                patchDoc.ApplyTo(taskUser, ModelState);

                if (!TryValidateModel(taskUser))
                {
                    return ValidationProblem(ModelState);
                }

                _taskUserService.UpdateTaskUser(id,taskUser);
                return Ok(new { message = "Cập nhật một phần thông tin TaskUser thành công" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật TaskUser" });
            }
        }
        // Xóa TaskUser theo ID
        [HttpDelete("{id}")]
        public IActionResult DeleteTaskUser(int id)
        {
            try
            {
                bool result = _taskUserService.DeleteTaskUser(id);
                if (!result)
                {
                    return NotFound(new { message = "TaskUser không tồn tại" });
                }

                return Ok(new { message = "Xóa TaskUser thành công" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể xóa TaskUser" });
            }
        }


        [HttpPost]
        public IActionResult AssignTaskToUser(TaskUser[] taskUsers)
        {

            return Ok(_taskService.AssignTaskToUser(taskUsers));
        }
        [HttpPut]
        public ActionResult UpdateAssignTaskToUser(TaskUser taskUser)
        {
            try
            {
                return Ok(_taskService.UpdateAssignTaskToUser(taskUser));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "TaskUser is not update" });
            }
        }
        [HttpDelete]
        public ActionResult DeleteAssignTaskToUser(long id)
        {
            try
            {
                return Ok(_taskService.DeleteAssignTaskToUser(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "TaskUser is not delete" });
            }
        }
    }
}
