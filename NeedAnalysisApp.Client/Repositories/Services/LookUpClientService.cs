namespace NeedAnalysisApp.Client.Repositories.Services;

public class LookUpClientService : ILookUpClientService
{
    private readonly HttpClient _httpClient;

    public LookUpClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<LookUpType>> GetAllTypes()
    {
        var response = await _httpClient.GetAsync("https://localhost:7028/api/industries");

        if (response.IsSuccessStatusCode)
        {
            var industries = await response.Content.ReadFromJsonAsync<List<LookUpType>>();
            return industries ?? new List<LookUpType>();
        }
        else
        {
            throw new HttpRequestException($"Failed to retrieve industries. Status code: {response.StatusCode}");
        }
    }
}
