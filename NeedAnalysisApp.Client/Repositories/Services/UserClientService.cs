namespace NeedAnalysisApp.Client.Repositories.Services;

public class UserClientService : IUserClientService
{
    #region Fields

    private readonly HttpClient _httpClient;

    #endregion

    #region Ctor

    public UserClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    #endregion

    #region Methods

    public async Task<List<UserDto>> GetAllAsync(string? role)
    {
        //var response = string.IsNullOrWhiteSpace(role)
        //    ? await _httpClient.GetAsync($"https://localhost:7028/api/users")
        //    : await _httpClient.GetAsync($"https://localhost:7028/api/users?role={role}");

        var response = string.IsNullOrWhiteSpace(role)
            ? await _httpClient.GetAsync(User.GetAll)
            : await _httpClient.GetAsync(string.Format(User.GetAllWithRole, role));

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

    public async Task<UserDto> GetWithIdAsync(string userId)
    {
        //var response = await _httpClient.GetAsync($"https://localhost:7028/api/users/{userId}");
        var response = await _httpClient.GetAsync(string.Format(User.GetWithId, userId));

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

    #endregion
}