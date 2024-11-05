namespace NeedAnalysisApp.Controllers;

[ApiController]
public class FilesController : ControllerBase
{
    private readonly IFileService _fileService;

    public FilesController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("api/files")]
    public async Task<FileDto> Upload(IFormFile file)
    {
        return await _fileService.Upload(file);
    }

}
