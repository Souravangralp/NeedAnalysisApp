using Microsoft.AspNetCore.Components.Forms;
using NeedAnalysisApp.Shared.Dto.Chat;

namespace NeedAnalysisApp.Client.Repositories.Interfaces;

public interface IFilesClientService
{
    Task<FileDto> Upload(IBrowserFile browserFile);
    Task<FileDto> Get(string fileId);
}
