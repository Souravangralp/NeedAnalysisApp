namespace NeedAnalysisApp.Repositories.Services;

public class FileService : IFileService
{
    public Task<FileDto> Get(string fileId)
    {
        throw new NotImplementedException();
    }

    public async Task<FileDto> Upload([FromForm] IFormFile file)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", file.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return new FileDto() { FileName = file.FileName, FileType = file.ContentType, FileUrl = $"images/{file.FileName}" };
    }
}
