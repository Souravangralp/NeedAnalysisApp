using Microsoft.AspNetCore.Mvc;
using NeedAnalysisApp.Repositories.Interfaces;
using NeedAnalysisApp.Shared.Dto;

namespace NeedAnalysisApp.Controllers;

[ApiController]
public class LookUpsController : ControllerBase
{
    private readonly ILookUpService _lookUpService;

    public LookUpsController(ILookUpService lookUpService)
    {
        _lookUpService = lookUpService;
    }

    [HttpGet("api/lookUps")]
    public Task<List<LookUpType>> GetAllTypes(string? type) 
    {
        return _lookUpService.GetAllTypes(type);
    }

    [HttpGet("api/lookUps/values")]
    public Task<List<LookUpType>> GetAllValuesWithType(string type)
    {
        return _lookUpService.GetAllValuesWithType(type);
    }


}
