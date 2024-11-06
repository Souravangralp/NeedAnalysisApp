namespace NeedAnalysisApp.Client.Repositories.Interfaces;

public interface IIndustryClientService
{
    Task<Result> CreateAsync(IndustryDto industry);

    Task<List<IndustryDto>> GetAllAsync();

    Task<Result> UpdateAsync(IndustryDto industry);

    Task<Result> DeleteAsync(string uniqueId);
}