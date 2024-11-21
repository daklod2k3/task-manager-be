using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Interfaces;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskCompleteController
{
    private readonly IRepository<TaskEntity> _tasks;
    private readonly string _uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");


    public TaskCompleteController(IUnitOfWork unitOfWork)
    {
        _tasks = unitOfWork.Task;
        if (!Directory.Exists(_uploadDirectory)) Directory.CreateDirectory(_uploadDirectory);
    }

    public async Task<IActionResult> MarkComplete([FromForm] IFormFile file)
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

            return new SuccessResponse<object>(new { FileName = uniqueFileName, FilePath = filePath });
        }
        catch (Exception ex)
        {
            // Handle errors
            return new ErrorResponse("File upload failed." + ex.Message);
        }
    }
}