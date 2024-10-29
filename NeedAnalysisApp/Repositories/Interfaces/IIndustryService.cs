using NeedAnalysisApp.Shared.Common;
using NeedAnalysisApp.Shared.Dto;

namespace NeedAnalysisApp.Repositories.Interfaces;

public interface IIndustryService
{
    Task<List<IndustryDto>> GetAll();

    Task<Result> Create(IndustryDto industry);

    Task<Result> Update(IndustryDto industry);

    Task<Result> Delete(string id);
}