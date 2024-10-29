using NeedAnalysisApp.Shared.Common;
using NeedAnalysisApp.Shared.Dto;

namespace NeedAnalysisApp.Repositories.Interfaces;

public interface IOptionService
{
    Task<Result> Create(string questionId, OptionDto option);
    Task<Result> Delete(string optionId);
}
