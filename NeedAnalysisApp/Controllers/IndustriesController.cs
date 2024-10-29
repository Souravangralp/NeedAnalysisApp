using Microsoft.AspNetCore.Mvc;
using NeedAnalysisApp.Repositories.Interfaces;
using NeedAnalysisApp.Shared.Common;
using NeedAnalysisApp.Shared.Dto;

namespace NeedAnalysisApp.Controllers;

//[Route("api/industries")]
[ApiController]
public class IndustriesController : ControllerBase
{
    private readonly IIndustryService _industryService;

    public IndustriesController(IIndustryService industryService)
    {
        _industryService = industryService;
    }

    [HttpGet("api/industries")]
    public async Task<List<IndustryDto>> GetAll()
    {
        return await _industryService.GetAll();
    }

    [HttpPost("api/industries")]
    public async Task<Result> Create(IndustryDto industryDto)
    {
        return await _industryService.Create(industryDto);
    }

    [HttpPatch("api/industries")]
    public async Task<Result> Update(IndustryDto industryDto)
    {
        return await _industryService.Update(industryDto);
    }

    [HttpDelete("api/industries/{uniqueId}")]
    public async Task<Result> Delete(string uniqueId) 
    {
        return await _industryService.Delete(uniqueId);
    }
}
