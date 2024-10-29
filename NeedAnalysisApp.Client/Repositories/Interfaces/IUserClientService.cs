using NeedAnalysisApp.Shared.Dto;

namespace NeedAnalysisApp.Client.Repositories.Interfaces;

public interface IUserClientService
{
    Task<List<UserDto>> GetAll(string? role);
    Task<UserDto> GetWithId(string id);
}
