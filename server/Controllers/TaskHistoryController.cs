using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using server.Context;
namespace server.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class TaskHistoryController : Controller
    {
        private readonly ITaskHistoryService _taskHistoryService;
        private readonly SupabaseContext _context;

        public TaskHistoryController(SupabaseContext context,ITaskHistoryService taskHistoryService)
        {
            _taskHistoryService = taskHistoryService;
            _context = context;
        }

        //phuong thuc get
        [HttpGet]
        public IActionResult GetAllTaskHistory()
        {
            try
            {
                return new SuccessResponse<IEnumerable<TaskHistory>>(_taskHistoryService.GetAllTaskHistory());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new ErrorResponse("TaskHistory is not found");
            }
        }

        //phuong thuc get theo id
        [HttpGet("{id}")]
        public IActionResult GetTaskHistoryById(long id)
        {
            try
            {
                return new SuccessResponse<TaskHistory>(_taskHistoryService.GetTaskHistoryById(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new ErrorResponse("TaskHistory is not found");
            }
        }

        //phuong thuc post tao 1 taskHistory
        [HttpPost]
        public IActionResult CreateTaskHistory(TaskHistory taskHistory)
        {
            try
            {
                return new SuccessResponse<TaskHistory>(_taskHistoryService.CreateTaskHistory(taskHistory));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new ErrorResponse("TaskHistory is not found");
            }
        }

        //phuong thuc PUT update 1 taskHistory
        [HttpPut("{id}")]
        public IActionResult UpdateTaskHistoryById(long id, TaskHistory taskHistory)
        {
            try
            {
                return new SuccessResponse<TaskHistory>(_taskHistoryService.UpdateTaskHistoryById(id, taskHistory));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new ErrorResponse("TaskHistory is not found");
            }
        }

        //phuong thuc patch update 1 taskHistory
        [HttpPatch("{id}")]
        public IActionResult PatchTaskHistoryById(long id, [FromBody] JsonPatchDocument<TaskHistory> taskHistory)
        {
            try
            {
                return new SuccessResponse<TaskHistory>(_taskHistoryService.PatchTaskHistoryById(id, taskHistory));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new ErrorResponse("TaskHistory is not found");
            }
        }

        //phuong thuc delete 1 taskHistory
        [HttpDelete("{id}")]
        public IActionResult DeleteTaskHistoryById(long id)
        {
            try
            {
                return new SuccessResponse<TaskHistory>(_taskHistoryService.DeleteTaskHistoryById(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new ErrorResponse("TaskHistory is not found");
            }
        }

    }
}