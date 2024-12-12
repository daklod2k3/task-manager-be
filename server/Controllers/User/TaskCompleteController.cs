using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;

namespace server.Controllers.User;

[ApiController]
[Route("/user/[controller]")]
public class TaskCompleteController : Controller
{
    private readonly IFileRepository _files;
    private readonly ITaskRepository _tasks;
    private readonly ITaskUserRepository _users;
    private readonly IUnitOfWork _unitOfWork;
    private readonly string _uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

    public TaskCompleteController(IUnitOfWork unitOfWork)
    {
        _tasks = unitOfWork.Tasks;
        _files = unitOfWork.Files;
        _users = unitOfWork.TaskUsers;
        _unitOfWork = unitOfWork;
        if (!Directory.Exists(_uploadDirectory)) Directory.CreateDirectory(_uploadDirectory);
    }

    [HttpPost("preview/{taskId}")]
    public async Task<IActionResult> MarkPreview(string taskId, [FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0) return new ErrorResponse("No file uploaded.");
        var idc = AuthController.GetUserId(HttpContext);
        var tuser = _users.Get(tu => tu.TaskId == long.Parse(taskId) && tu.UserId == new Guid(idc)).FirstOrDefault();
        if (tuser == null) return new ErrorResponse("You can't change this");

        try
        {
            // Generate a unique filename to avoid conflicts
            var uniqueFileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(_uploadDirectory, uniqueFileName);

            // Save the file to the server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var id = AuthController.GetUserId(HttpContext);
            var fileEntity = _files.Add(new FileEntity
            {
                CreatedBy = new Guid(id),
                Path = uniqueFileName
            });
            _unitOfWork.Save();
            var task = _tasks.GetById(taskId);
            task.Status = ETaskStatus.In_preview;
            task.FileId = fileEntity.Id;
            _unitOfWork.Save();


            return new SuccessResponse<TaskEntity>(task);
        }
        catch (Exception ex)
        {
            // Handle errors
            return new ErrorResponse("File upload failed." + ex.Message);
        }
    }

    [HttpPost("complete/{taskId}")]
    public async Task<IActionResult> MarkComplete(string taskId)
    {
        var task = _tasks.GetById(taskId);
        if (task is null) return new ErrorResponse("No task found.");
        if (task.FileId is null) return new ErrorResponse("Preview file not found.");
        var idc = AuthController.GetUserId(HttpContext);
        var tuser = _users.Get(tu => tu.TaskId == long.Parse(taskId) && tu.UserId == new Guid(idc)).FirstOrDefault();
        if (tuser == null) return new ErrorResponse("You can't change this");
        task.Status = ETaskStatus.Done;
        _unitOfWork.Save();
        return new SuccessResponse<TaskEntity>(task);
    }
}