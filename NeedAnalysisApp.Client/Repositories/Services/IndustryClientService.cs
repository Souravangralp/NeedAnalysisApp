using NeedAnalysisApp.Client.Repositories.Interfaces;
using System.Net.Http.Json;
using NeedAnalysisApp.Shared.Common;
using NeedAnalysisApp.Shared.Dto;
using System.Text;
using System.Text.Json;

namespace NeedAnalysisApp.Client.Repositories.Services;

public class IndustryClientService : IIndustryClientService
{
    private readonly HttpClient _httpClient;

    public IndustryClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result> Create(IndustryDto industry)
    {
        var response = await _httpClient.PostAsJsonAsync("https://localhost:7028/api/industries", industry);

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

    public async Task<Result> Delete(string uniqueId)
    {
        var response = await _httpClient.DeleteAsync($"https://localhost:7028/api/industries/{uniqueId}");

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

    public async Task<List<IndustryDto>> GetAll()
    {
        var response = await _httpClient.GetAsync("https://localhost:7028/api/industries");

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

    public async Task<Result> Update(IndustryDto industry)
    {
        var url = "https://localhost:7028/api/industries";
        var jsonContent = JsonSerializer.Serialize(industry);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // Send the PATCH request
        var response = await _httpClient.PatchAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Result>() ?? new Result(); // Indicate success
        }
        else
        {
            throw new HttpRequestException($"Failed to update industry. Status code: {response.StatusCode}");
        }
    }
}
