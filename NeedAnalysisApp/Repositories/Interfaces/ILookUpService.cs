using NeedAnalysisApp.Shared.Dto;

namespace NeedAnalysisApp.Repositories.Interfaces;

public interface ILookUpService
{
    Task<List<LookUpType>> GetAllTypes(string? type);

    Task<List<LookUpType>> GetAllValuesWithType(string type);
}
