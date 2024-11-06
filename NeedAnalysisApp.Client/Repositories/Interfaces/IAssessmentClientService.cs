namespace NeedAnalysisApp.Client.Repositories.Interfaces;

public interface IAssessmentClientService
{
    Task<Result> CreateAsync(AssessmentDto assessmentDto);

    Task<List<AssessmentDto>> GetAllAsync();

    Task<AssessmentDto> GetWithIdAsync(string assessmentId);

    Task<Result> UpdateAsync(AssessmentDto assessmentDto);

    Task<Result> DeleteAsync(string assessmentId);

    Task<Result> CreateScoreCategoriesAsync(List<ScoreCategoryDto> scoreCategories);

    Task<Result> CreateScoreCategoryAsync(ScoreCategoryDto scoreCategory);

    Task<Result> GetAllScoreCategoryAsync();

    Task<Result> GetScoreCategoryWithIdAsync(string scoreCategoryId);

    Task<Result> UpdateScoreCategoryAsync(ScoreCategoryDto scoreCategory);

    Task<Result> AssignAssessmentAsync(string assessmentId, string userId);

    Task<Result> GetUserAssessmentAsync(string userId);
}
