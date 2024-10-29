using NeedAnalysisApp.Shared.Common;
using NeedAnalysisApp.Shared.Dto;

namespace NeedAnalysisApp.Repositories.Interfaces;

public interface IAssessmentService
{
    Task<List<AssessmentDto>> GetAll();

    Task<AssessmentDto> GetWithId(string uniqueId);

    Task<Result> Create(AssessmentDto assessmentDto);

    Task<Result> Update(AssessmentDto assessmentDto);

    Task<Result> Delete(string uniqueId);

    Task<Result> GetAllScoreCategory();

    Task<Result> CreateScoreCategories(List<ScoreCategoryDto> scoreCategories);

    Task<Result> GetScoreCategoryWithId(string uniqueId);

    Task<Result> CreateScoreCategory(ScoreCategoryDto scoreCategories);

    Task<Result> UpdateScoreCategory(ScoreCategoryDto scoreCategories);

    Task<Result> AssignAssessment(string assessmentId, string userId);

    Task<Result> GetUserAssessment(string userId);
}