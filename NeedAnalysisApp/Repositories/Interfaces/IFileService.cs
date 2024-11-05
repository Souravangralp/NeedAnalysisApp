using NeedAnalysisApp.Shared.Dto.Chat;

namespace NeedAnalysisApp.Repositories.Interfaces;

public interface IFileService
{
    Task<FileDto> Upload(IFormFile file);
    Task<FileDto> Get(string fileId);
}
