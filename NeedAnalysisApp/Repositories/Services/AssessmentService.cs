using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using NeedAnalysisApp.Data;
using NeedAnalysisApp.Data.Models;
using NeedAnalysisApp.Data.Models.Assessment;
using NeedAnalysisApp.Data.Models.Scores;
using NeedAnalysisApp.Repositories.Interfaces;
using NeedAnalysisApp.Shared.Common;
using NeedAnalysisApp.Shared.Dto;

namespace NeedAnalysisApp.Repositories.Services;

public class AssessmentService : IAssessmentService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AssessmentService(ApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result> Create(AssessmentDto assessmentDto)
    {
        List<Error> errors = [];

        var industry = await _context.Industries.Where(i => i.Name.Trim().ToLower() == assessmentDto.IndustryType.Trim().ToLower()).FirstOrDefaultAsync();

        if (industry == null)
        {
            errors.Add(new Error() { Message = $"Apologizes for incontinence! but we do not have a industry type {assessmentDto.IndustryType}." });
        }

        var existingAssessment = await _context.Assessments.Where(i => i.Name.Trim().ToLower() == assessmentDto.Name.Trim().ToLower() && i.Assessment_IndustryID.Equals(industry.Id)).Include(x => x.Assessment_Industry).FirstOrDefaultAsync();

        if (existingAssessment != null) { errors.Add(new Error() { Message = $"Apologizes for incontinence! but we already have a assessment with : {assessmentDto.Name} with industryType: {existingAssessment.Assessment_Industry?.Name}. Please add a new assesment, thanks." }); };

        if (errors.Any()) { return new Result() { Success = false, Errors = errors, Model = null }; }

        var newAssessment = _mapper.Map<Assessment>(assessmentDto);

        newAssessment.Assessment_IndustryID = industry.Id;

        try
        {
            await _context.Assessments.AddAsync(newAssessment);

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw;
        }

        var test = _mapper.Map<AssessmentDto>(newAssessment);

        test.IndustryType = assessmentDto.IndustryType;

        return new Result() { Success = true, Errors = errors, Model = test };
    }

    public async Task<Result> Delete(string uniqueId)
    {
        List<Error> errors = [];

        var existingAssessment = await _context.Assessments.Where(i => i.UniqueId.Equals(uniqueId)).Include(x => x.Questions).ThenInclude(x => x.Options).FirstOrDefaultAsync();

        if (existingAssessment is null) { errors.Add(new Error() { Message = $"Apologizes for incontinence! but we have not found any assessment with Id: {uniqueId}. Please enter a valid record to update, thanks." }); }

        if (errors.Any()) { return new Result() { Success = false, Errors = errors, Model = null }; }

        try
        {
            if (existingAssessment.Questions.Any())
            {
                List<Option> options = [];

                foreach (var option in existingAssessment.Questions)
                {
                    options.AddRange(option.Options);
                }

                if (options.Any())
                {
                    _context.Options.RemoveRange(options);

                    await _context.SaveChangesAsync();
                }

                _context.Questions.RemoveRange(existingAssessment.Questions);

                await _context.SaveChangesAsync();
            }

            _context.Assessments.Remove(existingAssessment);

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            errors.Add(new Error() { Message = $"Apologize for the inconvenience, but we are facing issue while removing this assessment : {existingAssessment.Name}, Please contact technical support team. Thanks!" });
        }

        return new Result() { Success = errors.Any() ? false : true, Errors = errors, Model = null };
    }

    public async Task<List<AssessmentDto>> GetAll()
    {
        var assessments = await _context.Assessments.Where(i => !i.IsDeleted).Include(x => x.Assessment_Industry).ToListAsync();

        List<AssessmentDto> assessmentDtos = [];

        foreach (var assessment in assessments)
        {
            var industryType = assessment.Assessment_Industry?.Name;

            var assessmentDto = _mapper.Map<AssessmentDto>(assessment);

            assessmentDto.IndustryType = industryType;

            assessmentDtos.Add(assessmentDto);
        }

        return assessmentDtos;
    }

    public async Task<AssessmentDto> GetWithId(string uniqueId)
    {
        var assessment = await _context.Assessments.Where(x => x.UniqueId == uniqueId).Include(x => x.Assessment_Industry).FirstOrDefaultAsync();

        var newAssessment = _mapper.Map<AssessmentDto>(assessment);

        newAssessment.IndustryType = assessment?.Assessment_Industry?.Name ?? string.Empty;

        return newAssessment;
    }

    public async Task<Result> Update(AssessmentDto assessmentDto)
    {
        List<Error> errors = [];

        var existingIndustry = await _context.Industries.Where(i => i.Name.Equals(assessmentDto.Name) || i.Code.Equals(assessmentDto.TotalScore)).FirstOrDefaultAsync();

        if (existingIndustry != null && existingIndustry.UniqueId != assessmentDto.UniqueId) { errors.Add(new Error() { Message = $"Apologizes for incontinence! but we already have a assessment with name: {assessmentDto.Name} || Code: {assessmentDto.IndustryType}. Please add a unique one, thanks." }); };

        var toBeUpdatedAssessment = await _context.Assessments.Where(i => i.UniqueId.Equals(assessmentDto.UniqueId)).FirstOrDefaultAsync();

        if (toBeUpdatedAssessment is null) { errors.Add(new Error() { Message = $"Apologizes for incontinence! but we have not found any assessment with Id: {assessmentDto.UniqueId}. Please enter a valid record to update, thanks." }); }

        if (errors.Any()) { return new Result() { Success = false, Errors = errors, Model = null }; }

        toBeUpdatedAssessment.Description = assessmentDto.Description;
        toBeUpdatedAssessment.IsActive = assessmentDto.IsActive;
        toBeUpdatedAssessment.Title = assessmentDto.Title;
        toBeUpdatedAssessment.IsLive = assessmentDto.IsLive;
        toBeUpdatedAssessment.TotalScore = assessmentDto.TotalScore;

        _context.Assessments.Update(toBeUpdatedAssessment);

        await _context.SaveChangesAsync();

        return new Result() { Success = true, Errors = errors, Model = assessmentDto };
    }

    public async Task<Result> GetAllScoreCategory()
    {
        var scoreCategories = await _context.ScoreCategories.Where(x => x.IsActive).ToListAsync();

        if (scoreCategories == null) { return new Result() { Success = false, Model = null, Errors = { new Error() { Message = "We have not found any score categories please create!" } } }; }

        var scoreCategoriesDto = _mapper.Map<List<ScoreCategoryDto>>(scoreCategories);

        return new Result() { Success = true, Model = scoreCategoriesDto, Errors = [] };
    }

    public async Task<Result> CreateScoreCategories(List<ScoreCategoryDto> scoreCategories)
    {
        List<Error> errors = [];
        List<ScoreCategoryDto> newScoreCategoriesDto = [];

        try
        {
            var existingScoreCategories = await _context.ScoreCategories.Where(x => x.IsActive).ToListAsync();

            if (existingScoreCategories != null)
            {
                existingScoreCategories.ForEach(x => x.IsActive = false);

                _context.UpdateRange(existingScoreCategories);

                await _context.SaveChangesAsync();
            }

            var newScoreCategories = _mapper.Map<List<ScoreCategory>>(scoreCategories);

            await _context.ScoreCategories.AddRangeAsync(newScoreCategories);

            await _context.SaveChangesAsync();

            newScoreCategoriesDto = _mapper.Map<List<ScoreCategoryDto>>(newScoreCategories);
        }
        catch (Exception ex)
        {
            errors.Add(new Error() { Message = ex.Message });
        }

        if (errors.Count > 0) { return new Result() { Success = false, Errors = errors, Model = null }; }

        return new Result() { Success = true, Errors = errors, Model = newScoreCategoriesDto };

    }

    public async Task<Result> GetScoreCategoryWithId(string uniqueId)
    {
        var scoreCategory = await _context.ScoreCategories.FirstOrDefaultAsync(x => x.UniqueId == uniqueId);

        if (scoreCategory == null) { return new Result() { Success = false, Model = null, Errors = { new Error() { Message = $"We have not found any score categories with Id : {uniqueId} Please select appropriate id before getting the score!" } } }; }

        var scoreCategoryDto = _mapper.Map<ScoreCategoryDto>(scoreCategory);

        return new Result() { Success = true, Model = scoreCategoryDto, Errors = [] };
    }

    public async Task<Result> CreateScoreCategory(ScoreCategoryDto scoreCategory)
    {
        List<Error> errors = [];

        var existingScoreCategory = await _context.ScoreCategories.Where(x => x.Value.Trim().ToLower() == scoreCategory.Value.Trim().ToLower() && x.IsActive).FirstOrDefaultAsync();

        if (existingScoreCategory != null) { errors.Add(new Error() { Message = $"Apologizes for incontinence! but we have found a similar Score category Please choose other Value : For Category {scoreCategory.Value}. " }); }

        if (errors.Any()) { return new Result() { Errors = errors, Model = null, Success = false }; }

        var newScoreCategory = _mapper.Map<ScoreCategory>(scoreCategory);

        await _context.ScoreCategories.AddAsync(newScoreCategory);

        await _context.SaveChangesAsync();

        return new Result() { Success = true, Model = _mapper.Map<ScoreCategoryDto>(newScoreCategory), Errors = errors };
    }

    public async Task<Result> UpdateScoreCategory(ScoreCategoryDto scoreCategory)
    {
        List<Error> errors = [];

        var existingScoreCategory = await _context.ScoreCategories.Where(x => x.UniqueId == scoreCategory.UniqueId).FirstOrDefaultAsync();

        if (existingScoreCategory == null) { errors.Add(new Error() { Message = $"Apologizes for incontinence! but we have not found a Score category with given category : {scoreCategory.Value}. " }); }

        if (errors.Any()) { return new Result() { Errors = errors, Model = null, Success = false }; }

        var toBeUpdateCategory = _mapper.Map(scoreCategory, existingScoreCategory);

        _context.ScoreCategories.Update(toBeUpdateCategory);

        await _context.SaveChangesAsync();

        return new Result() { Success = true, Model = _mapper.Map<ScoreCategoryDto>(toBeUpdateCategory), Errors = errors };
    }

    public async Task<Result> AssignAssessment(string assessmentId, string userId)
    {
        List<Error> errors = [];

        // get assessment & validate if it exist
        var assessment = await _context.Assessments.Where(x => x.UniqueId == assessmentId).FirstOrDefaultAsync();

        if (assessment == null) { errors.Add(new Error() { Message = $"Apologize for inconvenience, but we have not found any assessment with this Id : {assessmentId}" }); }

        if (errors.Any()) { return new Result() { Success = false, Errors = errors, Model = null }; }

        // get all user assessment and check if assessment user wants to add already exist or not
        var userAssessmentMapper = await _context.UserAssessmentMapper.Where(x => x.UserAssessmentMapper_UserId == userId).ToListAsync();

        if (userAssessmentMapper.Any())
        {
            foreach (var userAssessment in userAssessmentMapper)
            {
                if (assessmentId == userAssessment.UniqueId) { errors.Add(new Error() { Message = $"We have found this assessment already available inside your bag! please choose a new one, thanks." }); }
            }
        }

        if (errors.Any()) { return new Result() { Success = false, Errors = errors, Model = null }; }

        // save a new assessment to user bag

        var newAssessment = new UserAssessmentMapper() { UserAssessmentMapper_UserId = userId, UserAssessmentMapper_AssessmentId = assessment.Id };

        await _context.UserAssessmentMapper.AddAsync(newAssessment);

        await _context.SaveChangesAsync();

        // return result
        return new Result() { Success = true, Errors = errors, Model = null };
    }

    public async Task<Result> GetUserAssessment(string userId)
    {
        var userAssessments = await _context.UserAssessmentMapper.Where(x => x.UserAssessmentMapper_UserId == userId).ToListAsync();

        List<Assessment> assessments = [];

        if (userAssessments.Any())
        {
            foreach (var userAssessment in userAssessments)
            {
                var assessment = await _context.Assessments.Where(i => !i.IsDeleted && i.Id == userAssessment.UserAssessmentMapper_AssessmentId).FirstOrDefaultAsync();

                if (assessment != null) { assessments.Add(assessment); }
            }
        }

        return new Result() { Errors = [], Model = assessments.Any() ? _mapper.Map<List<AssessmentDto>>(assessments) : null, Success = true };
    }
}
