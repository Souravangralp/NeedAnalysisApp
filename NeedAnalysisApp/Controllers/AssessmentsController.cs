using Microsoft.AspNetCore.Mvc;
using NeedAnalysisApp.Repositories.Interfaces;
using NeedAnalysisApp.Shared.Common;
using NeedAnalysisApp.Shared.Dto;

namespace NeedAnalysisApp.Controllers;

[ApiController]
public class AssessmentsController : ControllerBase
{
    private readonly IAssessmentService _assessmentService;

    public AssessmentsController(
        IAssessmentService assessmentService)
    {
        _assessmentService = assessmentService;
    }

    [HttpGet("api/assessment")]
    public async Task<List<AssessmentDto>> GetAll()
    {
        return await _assessmentService.GetAll();
    }

    [HttpGet("api/assessment/{id}")]
    public async Task<AssessmentDto> GetWithId(string id)
    {
        return await _assessmentService.GetWithId(id);
    }

    [HttpPost("api/assessment")]
    public async Task<Result> Create(AssessmentDto assessmentDto)
    {
        return await _assessmentService.Create(assessmentDto);
    }

    [HttpPatch("api/assessment")]
    public async Task<Result> Update(AssessmentDto assessmentDto)
    {
        return await _assessmentService.Update(assessmentDto);
    }

    [HttpDelete("api/assessment/{uniqueId}")]
    public async Task<Result> Delete(string uniqueId)
    {
        return await _assessmentService.Delete(uniqueId);
    }

    [HttpPost("api/assessment/assign/{userId}/{assessmentId}")]
    public async Task<Result> AssignAssessment(string userId, string assessmentId)
    {
        return await _assessmentService.AssignAssessment(assessmentId, userId);
    }

    [HttpGet("api/assessment/user/{userId}")]
    public async Task<Result> GetUserAssessment(string userId)
    {
        return await _assessmentService.GetUserAssessment(userId);
    }

    [HttpGet("api/assessment/scoreCategories")]
    public async Task<Result> GetAllScoreCategory()
    {
        return await _assessmentService.GetAllScoreCategory();
    }

    [HttpGet("api/assessment/scoreCategories/{uniqueId}")]
    public async Task<Result> GetScoreCategoryWithId(string uniqueId)
    {
        return await _assessmentService.GetScoreCategoryWithId(uniqueId);
    }

    [HttpPost("api/assessment/scoreCategories/add-new")]
    public async Task<Result> CreateScoreCategory(ScoreCategoryDto scoreCategory)
    {
        return await _assessmentService.CreateScoreCategory(scoreCategory);
    }

    [HttpPost("api/assessment/scoreCategories/add")]
    public async Task<Result> CreateScoreCategories(List<ScoreCategoryDto> scoreCategories)
    {
        return await _assessmentService.CreateScoreCategories(scoreCategories);
    }

    [HttpPost("api/assessment/scoreCategories/update")]
    public async Task<Result> UpdateScoreCategories(ScoreCategoryDto scoreCategories)
    {
        return await _assessmentService.UpdateScoreCategory(scoreCategories);
    }
}
