using System.Net.Http.Json;
using System.Xml;
using NeedAnalysisApp.Client.Repositories.Interfaces;
using NeedAnalysisApp.Shared.Common;
using NeedAnalysisApp.Shared.Dto;

namespace NeedAnalysisApp.Client.Repositories.Services;

public class AssessmentClientService : IAssessmentClientService
{
    private readonly HttpClient httpClient;

    public AssessmentClientService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    public async Task<Result> Create(AssessmentDto assessmentDto)
    {
        var response = await httpClient.PostAsJsonAsync("https://localhost:7028/api/assessment", assessmentDto);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result>(); // Deserializing the response to 'Result' object
            return result; // Return or use the result as needed
        }
        else
        {
            throw new HttpRequestException($"Failed to create industry. Status code: {response.StatusCode}");
        }
    }

    public async Task<Result> Delete(string uniqueId)
    {
        var response = await httpClient.DeleteAsync($"https://localhost:7028/api/assessment/{uniqueId}");

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

    public async Task<List<AssessmentDto>> GetAll()
    {
        var response = await httpClient.GetAsync("https://localhost:7028/api/assessment");

        if (response.IsSuccessStatusCode)
        {
            var assessments = await response.Content.ReadFromJsonAsync<List<AssessmentDto>>();
            return assessments ?? new List<AssessmentDto>();
        }
        else
        {
            throw new HttpRequestException($"Failed to retrieve industries. Status code: {response.StatusCode}");
        }
    }

    public async Task<AssessmentDto> GetWithId(string uniqueId)
    {
        try
        {
            var response = await httpClient.GetAsync($"https://localhost:7028/api/assessment/{uniqueId}");

            if (response.IsSuccessStatusCode)
            {
                var assessment = await response.Content.ReadFromJsonAsync<AssessmentDto>();
                return assessment ?? new AssessmentDto() { Name = "" };
            }
            else
            {
                throw new HttpRequestException($"Failed to retrieve industries. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public async Task<Result> Update(AssessmentDto assessmentDto)
    {
        var response = await httpClient.PatchAsJsonAsync("https://localhost:7028/api/assessment", assessmentDto);

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

    public async Task<Result> GetAllScoreCategory()
    {
        try
        {
            var response = await httpClient.GetAsync($"https://localhost:7028/api/assessment/scoreCategories");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Result>() ?? new Result();
            }
            else
            {
                throw new HttpRequestException($"Failed to retrieve industries. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public async Task<Result> CreateScoreCategories(List<ScoreCategoryDto> scoreCategories)
    {
        var response = await httpClient.PostAsJsonAsync("https://localhost:7028/api/assessment/scoreCategories/add", scoreCategories);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Result>() ?? new Result(); // Deserializing the response to 'Result' object
        }
        else
        {
            throw new HttpRequestException($"Failed to create industry. Status code: {response.StatusCode}");
        }
    }

    public async Task<Result> GetScoreCategoryWithId(string uniqueId)
    {
        var response = await httpClient.GetAsync($"https://localhost:7028/api/assessment/scoreCategories/{uniqueId}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Result>() ?? new Result(); // Deserializing the response to 'Result' object
        }
        else
        {
            throw new HttpRequestException($"Failed to create industry. Status code: {response.StatusCode}");
        }
    }

    public async Task<Result> CreateScoreCategory(ScoreCategoryDto scoreCategory)
    {
        var response = await httpClient.PostAsJsonAsync("https://localhost:7028/api/assessment/scoreCategories/add-new", scoreCategory);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Result>() ?? new Result(); // Deserializing the response to 'Result' object
        }
        else
        {
            throw new HttpRequestException($"Failed to create industry. Status code: {response.StatusCode}");
        }
    }

    public async Task<Result> UpdateScoreCategory(ScoreCategoryDto scoreCategory)
    {
        var response = await httpClient.PostAsJsonAsync("https://localhost:7028/api/assessment/scoreCategories/update", scoreCategory);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Result>() ?? new Result(); // Deserializing the response to 'Result' object
        }
        else
        {
            throw new HttpRequestException($"Failed to create industry. Status code: {response.StatusCode}");
        }
    }

    public async Task<Result> AssignAssessment(string assessmentId, string userId)
    {
        var response = await httpClient.PostAsJsonAsync($"https://localhost:7028/api/assessment/assign/{userId}/{assessmentId}", new { });

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result>(); // Deserializing the response to 'Result' object
            return result; // Return or use the result as needed
        }
        else
        {
            throw new HttpRequestException($"Failed to create industry. Status code: {response.StatusCode}");
        }
    }

    public async Task<Result> GetUserAssessment(string userId)
    {
        var response = await httpClient.GetAsync($"https://localhost:7028/api/assessment/user/{userId}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Result>() ?? new Result(); // Deserializing the response to 'Result' object
        }
        else
        {
            throw new HttpRequestException($"Failed to create industry. Status code: {response.StatusCode}");
        }
    }
}
