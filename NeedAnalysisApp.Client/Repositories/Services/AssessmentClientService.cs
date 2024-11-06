namespace NeedAnalysisApp.Client.Repositories.Services;

public class AssessmentClientService : IAssessmentClientService
{
    #region Fields

    private readonly HttpClient _httpClient;

    #endregion

    #region Ctor

    public AssessmentClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    #endregion

    #region Methods

    public async Task<Result> CreateAsync(AssessmentDto assessmentDto)
    {
        //var response = await _httpClient.PostAsJsonAsync("https://localhost:7028/api/assessment", assessmentDto);
        var response = await _httpClient.PostAsJsonAsync(Assessment.Create, assessmentDto);

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

    public async Task<List<AssessmentDto>> GetAllAsync()
    {
        //var response = await _httpClient.GetAsync("https://localhost:7028/api/assessment");
        var response = await _httpClient.GetAsync(Assessment.GetAll);

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

    public async Task<AssessmentDto> GetWithIdAsync(string assessmentId)
    {
        try
        {
            //var response = await _httpClient.GetAsync($"https://localhost:7028/api/assessment/{assessmentId}");
            var response = await _httpClient.GetAsync(string.Format(Assessment.Get, assessmentId));

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

    public async Task<Result> UpdateAsync(AssessmentDto assessmentDto)
    {
        var response = await _httpClient.PatchAsJsonAsync(Assessment.Update, assessmentDto);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Result>();
        }
        else
        {
            throw new HttpRequestException($"Failed to create industry. Status code: {response.StatusCode}");
        }
    }

    public async Task<Result> DeleteAsync(string assessmentId)
    {
        //var response = await _httpClient.DeleteAsync($"https://localhost:7028/api/assessment/{assessmentId}");
        var response = await _httpClient.DeleteAsync(string.Format(Assessment.Delete, assessmentId));

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

    #region score category

    public async Task<Result> CreateScoreCategoriesAsync(List<ScoreCategoryDto> scoreCategories)
    {
        //var response = await _httpClient.PostAsJsonAsync("https://localhost:7028/api/assessment/scoreCategories/add", scoreCategories);
        var response = await _httpClient.PostAsJsonAsync(Assessment.ScoreCategory.Create, scoreCategories);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Result>() ?? new Result();
        }
        else
        {
            throw new HttpRequestException($"Failed to create industry. Status code: {response.StatusCode}");
        }
    }

    public async Task<Result> CreateScoreCategoryAsync(ScoreCategoryDto scoreCategory)
    {
        //var response = await _httpClient.PostAsJsonAsync("https://localhost:7028/api/assessment/scoreCategories/add-new", scoreCategory);
        var response = await _httpClient.PostAsJsonAsync(Assessment.ScoreCategory.CreateSingle, scoreCategory);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Result>() ?? new Result(); // Deserializing the response to 'Result' object
        }
        else
        {
            throw new HttpRequestException($"Failed to create industry. Status code: {response.StatusCode}");
        }
    }

    public async Task<Result> GetAllScoreCategoryAsync()
    {
        try
        {
            //var response = await _httpClient.GetAsync($"https://localhost:7028/api/assessment/scoreCategories");
            var response = await _httpClient.GetAsync(Assessment.ScoreCategory.GetAll);

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

    public async Task<Result> GetScoreCategoryWithIdAsync(string uniqueId)
    {
        //var response = await _httpClient.GetAsync($"https://localhost:7028/api/assessment/scoreCategories/{uniqueId}");
        var response = await _httpClient.GetAsync(string.Format(Assessment.ScoreCategory.Get, uniqueId));

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Result>() ?? new Result(); // Deserializing the response to 'Result' object
        }
        else
        {
            throw new HttpRequestException($"Failed to create industry. Status code: {response.StatusCode}");
        }
    }

    public async Task<Result> UpdateScoreCategoryAsync(ScoreCategoryDto scoreCategory)
    {
        //var response = await _httpClient.PostAsJsonAsync("https://localhost:7028/api/assessment/scoreCategories/update", scoreCategory);
        var response = await _httpClient.PostAsJsonAsync(Assessment.ScoreCategory.Update, scoreCategory);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Result>() ?? new Result(); // Deserializing the response to 'Result' object
        }
        else
        {
            throw new HttpRequestException($"Failed to create industry. Status code: {response.StatusCode}");
        }
    }

    #endregion

    #region assign

    public async Task<Result> AssignAssessmentAsync(string assessmentId, string userId)
    {
        //var response = await _httpClient.PostAsJsonAsync($"https://localhost:7028/api/assessment/assign/{userId}/{assessmentId}", new { });

        var response = await _httpClient.PostAsJsonAsync(string.Format(Assessment.User.Assign, userId, assessmentId), new { });

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

    public async Task<Result> GetUserAssessmentAsync(string userId)
    {
        //var response = await _httpClient.GetAsync($"https://localhost:7028/api/assessment/user/{userId}");
        var response = await _httpClient.GetAsync(string.Format(Assessment.User.Get, userId));

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Result>() ?? new Result(); // Deserializing the response to 'Result' object
        }
        else
        {
            throw new HttpRequestException($"Failed to create industry. Status code: {response.StatusCode}");
        }
    }

    #endregion

    #endregion
}