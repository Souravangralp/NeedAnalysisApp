using NeedAnalysisApp.Client.Repositories.Interfaces;
using NeedAnalysisApp.Shared.Common;
using NeedAnalysisApp.Shared.Dto;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace NeedAnalysisApp.Client.Repositories.Services;

public class UserClientService : IUserClientService
{
    private readonly HttpClient _httpClient;

    public UserClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<UserDto>> GetAll(string? role)
    {
        var response = string.IsNullOrWhiteSpace(role)
            ? await _httpClient.GetAsync($"https://localhost:7028/api/users")
            : await _httpClient.GetAsync($"https://localhost:7028/api/users?role={role}");

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result>();

            return JsonConvert.DeserializeObject<List<UserDto>>(result.Model.ToString() ?? string.Empty);
        }
        else
        {
            throw new HttpRequestException($"Failed to retrieve industries. Status code: {response.StatusCode}");
        }
    }

    public async Task<UserDto> GetWithId(string id)
    {
        var response = await _httpClient.GetAsync($"https://localhost:7028/api/users/{id}");

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result>();

            return JsonConvert.DeserializeObject<UserDto>(result.Model.ToString() ?? string.Empty);
        }
        else
        {
            throw new HttpRequestException($"Failed to retrieve industries. Status code: {response.StatusCode}");
        }
    }
}
