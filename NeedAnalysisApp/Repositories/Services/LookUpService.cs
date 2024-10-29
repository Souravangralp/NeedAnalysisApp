using Microsoft.EntityFrameworkCore;
using NeedAnalysisApp.Data;
using NeedAnalysisApp.Repositories.Interfaces;
using NeedAnalysisApp.Shared.Dto;

namespace NeedAnalysisApp.Repositories.Services;

public class LookUpService : ILookUpService
{
    private readonly ApplicationDbContext _context;

    public LookUpService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<LookUpType>> GetAllTypes(string? type)
    {
        var generalLookups = await _context.GeneralLookUps.Where(x => x.IsActive == true && !x.IsDeleted).ToListAsync();

        if (!string.IsNullOrWhiteSpace(type)) { generalLookups.Where(x => x.Type.Contains(type)).ToList(); }

        return generalLookups.Select(y => new LookUpType()
        {
            Id = y.Id,
            Type = y.Type
        }).DistinctBy(x => x.Type).ToList();
    }

    public async Task<List<LookUpType>> GetAllValuesWithType(string type)
    {
        return await _context.GeneralLookUps.Where(x => x.IsActive == true && !x.IsDeleted && x.Type.Trim().ToLower() == type.Trim().ToLower())
            .Select(y => new LookUpType()
            {
                Id = y.Id,
                Type = y.Value
            }).ToListAsync();
    }
}
