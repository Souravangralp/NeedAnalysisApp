namespace NeedAnalysisApp.Client.Repositories.Interfaces;

public interface IUserClientService
{
    Task<List<UserDto>> GetAllAsync(string? role);
    Task<UserDto> GetWithIdAsync(string id);
}
