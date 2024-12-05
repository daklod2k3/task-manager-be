using Microsoft.AspNetCore.Mvc;
using server.Context;
using server.Interfaces;
using server.Repository;

namespace server.Controllers;

[Route("[controller]")]
public class FileController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly string _uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");


    public FileController(SupabaseContext context)
    {
        _unitOfWork = new UnitOfWork(context);
    }

    [HttpGet]
    [Route("{file_id}")]
    public async Task<IActionResult> Index(string file_id)
    {
        var entity = _unitOfWork.Files.GetById(file_id);
        var file_path = Path.Combine(entity.Path, _uploadDirectory);
        if (!Directory.Exists(file_path))
            return new ErrorResponse("File does not exist");
        var stream = new FileStream(_uploadDirectory + "/" + entity.Path, FileMode.Open, FileAccess.Read,
            FileShare.Read);
        var contentType = GetContentType(stream.Name);

        // Return the file as a FileResult
        return File(stream, contentType, entity.Path);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    private string GetContentType(string path)
    {
        var types = new Dictionary<string, string>
        {
            { ".txt", "text/plain" },
            { ".pdf", "application/pdf" },
            { ".jpg", "image/jpeg" },
            { ".png", "image/png" },
            { ".doc", "application/msword" },
            { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { ".xls", "application/vnd.ms-excel" },
            { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" }
        };

        var ext = Path.GetExtension(path).ToLowerInvariant();
        return types.ContainsKey(ext) ? types[ext] : "application/octet-stream";
    }
}