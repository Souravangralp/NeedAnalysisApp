using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NeedAnalysisApp.Data;
using NeedAnalysisApp.Data.Models.Assessment;
using NeedAnalysisApp.Repositories.Interfaces;
using NeedAnalysisApp.Shared.Common;
using NeedAnalysisApp.Shared.Dto;

namespace NeedAnalysisApp.Repositories.Services;

public class QuestionService : IQuestionService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public QuestionService(ApplicationDbContext context,
        IMapper mapper
        )
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result> Create(QuestionDto questionDto, string assessmentId)
    {
        List<Error> errors = [];

        var assessment = await _context.Assessments.Where(x => x.UniqueId == assessmentId)
            .Include(x => x.Questions)
            .ThenInclude(x => x.Options)
            .FirstOrDefaultAsync();

        //var existingQuestion = assessment.Questions.FirstOrDefault(x=> x.Value.Equals(questionDto.Value) && x.GeneralLookUp_QuestionTypeId == questionDto.GeneralLookUp_QuestionTypeId);

        //if (existingQuestion != null) { errors.Add(new Error() { Message = $"Apologize for any inconvenience but we found the similar question with value : {existingQuestion.Value}, Please create another question." }); }

        //if (errors.Any()) { return new Result() { Success = false, Errors = errors, Model = null }; }

        var question = _mapper.Map<Question>(questionDto);

        question.Question_AssessmentId = assessment.Id;

        question.Options = _mapper.Map<List<Option>>(questionDto.Options);

        await _context.Questions.AddAsync(question);

        await _context.SaveChangesAsync();

        var createdQuestionDto = _mapper.Map<QuestionDto>(question);

        return new Result() { Success = true, Errors = errors, Model = createdQuestionDto };
    }

    public async Task<Result> GetAll(string assessmentId)
    {
        List<Error> errors = [];

        var assessment = await _context.Assessments.Where(x => x.UniqueId == assessmentId)
            .Include(x => x.Questions)
            .ThenInclude(x => x.Options)
            .FirstOrDefaultAsync();

        if (assessment == null) { errors.Add(new Error() { Message = $"Apologize for any inconvenience but we have not found any assessment with id : {assessmentId}, Please select appropriate assessment." }); }

        if (errors.Any()) { return new Result() { Success = false, Errors = errors, Model = null }; }

        //if (!assessment.Questions.Any()) { errors.Add(new Error() { Message = $"Apologize for any inconvenience but we do not found any question for assessment : {assessment.Name}, Please add questions." }); }

        //if (errors.Any()) { return new Result() { Success = false, Errors = errors, Model = null }; }

        if (assessment.Questions.Any())
        {
            var questionDto = _mapper.Map<List<QuestionDto>>(assessment.Questions);

            return new Result() { Success = true, Errors = errors, Model = questionDto };
        }
        else 
        {
            return new Result() { Success = true, Errors = errors, Model = new List<QuestionDto>() };
        }

    }

    public async Task<Result> GetWithId(string assessmentId, string questionId)
    {
        List<Error> errors = [];
        var assessment = await _context.Assessments.Where(x => x.UniqueId == assessmentId)
            .Include(x => x.Questions)
            .ThenInclude(x => x.Options)
            .FirstOrDefaultAsync();

        if (assessment == null) { errors.Add(new Error() { Message = $"Apologize for any inconvenience but we have not found any assessment with id : {assessmentId}, Please select appropriate assessment." }); }

        var question = assessment.Questions.Where(x => x.UniqueId.Equals(questionId)).FirstOrDefault();

        if (question == null) { errors.Add(new Error() { Message = $"Apologize for any inconvenience but we do not found any question for assessment : {assessment.Name}, Please try selecting another questions id." }); }

        if (errors.Any()) { return new Result() { Success = false, Errors = errors, Model = null }; }

        var questionDto = _mapper.Map<QuestionDto>(question);

        return new Result() { Success = true, Errors = errors, Model = questionDto };
    }

    public async Task<Result> Update(QuestionDto questionDto, string assessmentId)
    {
        List<Error> errors = [];

        var assessment = await _context.Assessments.Where(x => x.UniqueId == assessmentId)
            .Include(x => x.Questions)
            .ThenInclude(x => x.Options)
            .FirstOrDefaultAsync();

        var existingQuestion = assessment.Questions.Where(i => i.GeneralLookUp_SectionTypeId == questionDto.GeneralLookUp_SectionTypeId && i.Value.Equals(questionDto.Value) && i.GeneralLookUp_QuestionTypeId.Equals(questionDto.GeneralLookUp_QuestionTypeId) && i.UniqueId != questionDto.UniqueId).FirstOrDefault();

        if (existingQuestion != null && existingQuestion.UniqueId != questionDto.UniqueId) { errors.Add(new Error() { Message = $"Apologize for any inconvenience but we found the similar question with value : {existingQuestion.Value}, Please create another question." }); };

        var toBeUpdatedQuestion = await _context.Questions.Where(i => i.UniqueId.Equals(questionDto.UniqueId)).Include(x => x.Options).FirstOrDefaultAsync();

        if (toBeUpdatedQuestion is null) { errors.Add(new Error() { Message = $"Apologizes for incontinence! but we have not found any question with Id: {questionDto.UniqueId}. Please enter a valid record to update, thanks." }); }

        if (errors.Any()) { return new Result() { Success = false, Errors = errors, Model = null }; }

        if (toBeUpdatedQuestion.Options.Any()) 
        {
            foreach (var toBeUpdatedOptions in toBeUpdatedQuestion.Options)
            {
                foreach (var newOption in questionDto.Options)
                {
                    if (toBeUpdatedOptions.UniqueId == newOption.UniqueId) 
                    {
                        toBeUpdatedOptions.Value = newOption.Value;
                        toBeUpdatedOptions.DisplayOrder = newOption.DisplayOrder;
                        toBeUpdatedOptions.Point = newOption.Point;
                        toBeUpdatedOptions.ISInCorrectMatch = newOption.ISCorrect;

                        _context.Options.Update(toBeUpdatedOptions);
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }

        toBeUpdatedQuestion.DisplayOrder = questionDto.DisplayOrder;
        toBeUpdatedQuestion.Description = questionDto.Description;
        toBeUpdatedQuestion.Value= questionDto.Value;
        toBeUpdatedQuestion.GeneralLookUp_SectionTypeId = questionDto.GeneralLookUp_SectionTypeId;
        toBeUpdatedQuestion.GeneralLookUp_QuestionTypeId = questionDto.GeneralLookUp_QuestionTypeId;
        
        _context.Questions.Update(toBeUpdatedQuestion);

        await _context.SaveChangesAsync();

        return new Result() { Success = true, Errors = errors, Model = questionDto };

    }

    public async Task<Result> Delete(string uniqueId)
    {
        List<Error> errors = [];

        var existingQuestion = await _context.Questions.Where(i => i.UniqueId.Equals(uniqueId)).Include(x => x.Options).FirstOrDefaultAsync();

        if (existingQuestion is null) { errors.Add(new Error() { Message = $"Apologizes for incontinence! but we have not found any question with Id: {uniqueId}. Please enter a valid record to update, thanks." }); }

        if (errors.Any()) { return new Result() { Success = false, Errors = errors, Model = null }; }

        _context.Options.RemoveRange(existingQuestion.Options);

        await _context.SaveChangesAsync();

        _context.Questions.Remove(existingQuestion);

        await _context.SaveChangesAsync();

        return new Result() { Success = true, Errors = errors, Model = null };
    }
}
