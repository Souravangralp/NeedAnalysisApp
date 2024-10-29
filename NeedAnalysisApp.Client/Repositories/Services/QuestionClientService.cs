using NeedAnalysisApp.Client.Repositories.Interfaces;
using NeedAnalysisApp.Shared.Common;
using NeedAnalysisApp.Shared.Dto;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Xml;

namespace NeedAnalysisApp.Client.Repositories.Services;

public class QuestionClientService : IQuestionClientService
{
    private readonly HttpClient _httpClient;

    public QuestionClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result> Create(QuestionDto questionDto, string assessmentId)
    {
        var response = await _httpClient.PostAsJsonAsync($"https://localhost:7028/api/questions/{assessmentId}", questionDto);

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
        var response = await _httpClient.DeleteAsync($"https://localhost:7028/api/questions/{uniqueId}");

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

    public async Task<Result> GetAll(string assessmentId)
    {
        var response = await _httpClient.GetAsync($"https://localhost:7028/api/questions/{assessmentId}");

        if (response.IsSuccessStatusCode)
        {
            var result= await response.Content.ReadFromJsonAsync<Result>();
            return result ?? new Result();
        }
        else
        {
            throw new HttpRequestException($"Failed to retrieve industries. Status code: {response.StatusCode}");
        }
    }

    public async Task<Result> GetWithId(string assessmentId, string questionId)
    {
        var response = await _httpClient.GetAsync($"https://localhost:7028/api/questions/{assessmentId}/{questionId}");

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result>();
            return result ?? new Result();
        }
        else
        {
            throw new HttpRequestException($"Failed to retrieve industries. Status code: {response.StatusCode}");
        }
    }

    public async Task<Result> Update(QuestionDto questionDto, string assessmentId)
    {
        var url = $"https://localhost:7028/api/questions/{assessmentId}";
        var jsonContent = JsonSerializer.Serialize(questionDto);
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


    public async Task<Result> DeleteOption(string optionId)
    {
        var response = await _httpClient.DeleteAsync($"https://localhost:7028/api/options/{optionId}");

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


    public async Task<Result> CreateOption(string questionId, OptionDto option)
    {
        var response = await _httpClient.PostAsJsonAsync($"https://localhost:7028/api/options/{questionId}", option);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Result>();
        }
        else
        {
            throw new HttpRequestException($"Failed to create option. Status code: {response.StatusCode}");
        }
    }
}
