using NeedAnalysisApp.Data;
using NeedAnalysisApp.Shared.Dto;

namespace NeedAnalysisApp.Repositories.Interfaces;

public interface IUserService
{
    Task<List<UserDto>> GetAll(string? role);

    Task<UserDto> GetWithId(string Id);
}
