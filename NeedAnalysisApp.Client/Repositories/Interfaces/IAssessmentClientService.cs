namespace NeedAnalysisApp.Client.Repositories.Interfaces;

public interface IAssessmentClientService
{
    Task<List<AssessmentDto>> GetAll();

    Task<AssessmentDto> GetWithId(string uniqueId);

    Task<Result> Create(AssessmentDto assessmentDto);

    Task<Result> Update(AssessmentDto assessmentDto);

    Task<Result> Delete(string uniqueId);

    Task<Result> GetAllScoreCategory();

    Task<Result> CreateScoreCategories(List<ScoreCategoryDto> scoreCategories);

    Task<Result> GetScoreCategoryWithId(string uniqueId);

    Task<Result> CreateScoreCategory(ScoreCategoryDto scoreCategory);

    Task<Result> UpdateScoreCategory(ScoreCategoryDto scoreCategory);

    Task<Result> AssignAssessment(string assessmentId, string userId);

    Task<Result> GetUserAssessment(string userId);
}
