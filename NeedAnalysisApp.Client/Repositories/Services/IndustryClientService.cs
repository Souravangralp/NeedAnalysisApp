namespace NeedAnalysisApp.Client.Repositories.Services;

public class IndustryClientService : IIndustryClientService
{
    #region Fields

    private readonly HttpClient _httpClient;

    #endregion

    #region Ctor

    public IndustryClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    #endregion

    #region Methods

    public async Task<Result> CreateAsync(IndustryDto industry)
    {
        //var response = await _httpClient.PostAsJsonAsync("https://localhost:7028/api/industries", industry);
        var response = await _httpClient.PostAsJsonAsync(Industry.Create, industry);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Result>(); // Deserializing the response to 'Result' object
            //return result; // Return or use the result as needed
        }
        else
        {
            throw new HttpRequestException($"Failed to create industry. Status code: {response.StatusCode}");
        }

    }

    public async Task<List<IndustryDto>> GetAllAsync()
    {
        //var response = await _httpClient.GetAsync("https://localhost:7028/api/industries");
        var response = await _httpClient.GetAsync(Industry.GetAll);

        if (response.IsSuccessStatusCode)
        {
            var industries = await response.Content.ReadFromJsonAsync<List<IndustryDto>>();

            return industries ?? new List<IndustryDto>();
        }
        else
        {
            throw new HttpRequestException($"Failed to retrieve industries. Status code: {response.StatusCode}");
        }
    }

    public async Task<Result> UpdateAsync(IndustryDto industry)
    {
        var jsonContent = System.Text.Json.JsonSerializer.Serialize(industry);

        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.PatchAsync(Industry.Update, content);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Result>() ?? new Result();
        }
        else
        {
            throw new HttpRequestException($"Failed to update industry. Status code: {response.StatusCode}");
        }
    }

    public async Task<Result> DeleteAsync(string industryId)
    {
        //var response = await _httpClient.DeleteAsync($"https://localhost:7028/api/industries/{industryId}");
        var response = await _httpClient.DeleteAsync(string.Format(Industry.Delete, industryId));

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Result>();
        }
        else
        {
            throw new HttpRequestException($"Failed to create industry. Status code: {response.StatusCode}");
        }
    }

    #endregion
}
