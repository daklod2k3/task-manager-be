using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskCompleteController : Controller
{
    private readonly IFileRepository _files;
    private readonly ITaskRepository _tasks;
    private readonly IUnitOfWork _unitOfWork;
    private readonly string _uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

    public TaskCompleteController(IUnitOfWork unitOfWork)
    {
        _tasks = unitOfWork.Task;
        _files = unitOfWork.FileRepository;
        _unitOfWork = unitOfWork;
        if (!Directory.Exists(_uploadDirectory)) Directory.CreateDirectory(_uploadDirectory);
    }
    
    [HttpPost]
    [Route("{taskId}")]
    public async Task<IActionResult> MarkComplete(string taskId, [FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0) return new ErrorResponse("No file uploaded.");

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
}