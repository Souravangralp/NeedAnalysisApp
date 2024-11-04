namespace NeedAnalysisApp.Client.Repositories.Interfaces;

public interface IQuestionClientService
{
    Task<Result> GetAll(string assessmentId);

    Task<Result> GetWithId(string assessmentId, string questionId);

    Task<Result> Create(QuestionDto questionDto, string assessmentId);

    Task<Result> Update(QuestionDto questionDto, string assessmentId);

    Task<Result> Delete(string uniqueId);

    Task<Result> DeleteOption(string optionId);

    Task<Result> CreateOption(string questionId, OptionDto option);
}
