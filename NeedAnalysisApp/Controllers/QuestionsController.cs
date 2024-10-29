using Microsoft.AspNetCore.Mvc;
using NeedAnalysisApp.Repositories.Interfaces;
using NeedAnalysisApp.Shared.Common;
using NeedAnalysisApp.Shared.Dto;

namespace NeedAnalysisApp.Controllers;

[ApiController]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionService _questionService;
    private readonly IOptionService _optionService;

    public QuestionsController(IQuestionService questionService,
        IOptionService optionService)
    {
        _questionService = questionService;
        _optionService = optionService;
    }

    [HttpGet("api/questions/{assessmentId}")]
    public async Task<Result> GetAll(string assessmentId)
    {
        return await _questionService.GetAll(assessmentId);
    }

    [HttpGet("api/questions/{assessmentId}/{questionId}")]
    public async Task<Result> GetWithId(string assessmentId, string questionId)
    {
        return await _questionService.GetWithId(assessmentId, questionId);
    }

    [HttpPost("api/questions/{assessmentId}")]
    public async Task<Result> Create(QuestionDto questionDto, string assessmentId)
    {
        return await _questionService.Create(questionDto, assessmentId);
    }

    [HttpPatch("api/questions/{assessmentId}")]
    public async Task<Result> Update(QuestionDto questionDto, string assessmentId)
    {
        return await _questionService.Update(questionDto, assessmentId);
    }

    [HttpDelete("api/questions/{uniqueId}")]
    public async Task<Result> Delete(string uniqueId)
    {
        return await _questionService.Delete(uniqueId);
    }

    [HttpPost("api/options/{questionId}")]
    public async Task<Result> Create(string questionId, OptionDto optionDto)
    {
        return await _optionService.Create(questionId, optionDto);
    }

    [HttpDelete("api/options/{optionId}")]
    public async Task<Result> DeleteOption(string optionId)
    {
        return await _optionService.Delete(optionId);
    }
}
