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
        var entity = _unitOfWork.FileRepository.GetById(file_id);
        var file = entity.Path;
        var stream = new FileStream(_uploadDirectory + "/" + entity.Path, FileMode.Open, FileAccess.Read, FileShare.Read);
        if (stream == null)
            return new ErrorResponse("File not found");
        return File(stream, "application/octet-stream", entity.Path);
    }
}