using NeedAnalysisApp.Shared.Common;
using NeedAnalysisApp.Shared.Dto;

namespace NeedAnalysisApp.Repositories.Interfaces;

public interface IQuestionService
{
    Task<Result> GetAll(string assessmentId);

    Task<Result> GetWithId(string assessmentId, string questionId);

    Task<Result> Create(QuestionDto questionDto, string assessmentId);

    Task<Result> Update(QuestionDto questionDto, string assessmentId);

    Task<Result> Delete(string uniqueId);
}
