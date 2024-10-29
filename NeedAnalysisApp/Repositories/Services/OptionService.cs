using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NeedAnalysisApp.Data;
using NeedAnalysisApp.Data.Models.Assessment;
using NeedAnalysisApp.Repositories.Interfaces;
using NeedAnalysisApp.Shared.Common;
using NeedAnalysisApp.Shared.Dto;

namespace NeedAnalysisApp.Repositories.Services;

public class OptionService : IOptionService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public OptionService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result> Create(string questionId, OptionDto option)
    {
        List<Error> errors = [];

        var question = await _context.Questions.Where(x => x.UniqueId == questionId).FirstOrDefaultAsync();

        if (question == null) { errors.Add(new Error() { Message = $"Apologize for any inconvenience but unfortunately we have not found any question with Id : {questionId}. please choose another question id and try again."  }); }

        if (errors.Any()) { return new Result() { Errors = errors, Model = null, Success = false }; }

        var newOption = _mapper.Map<Option>(option);

        newOption.Option_QuestionID = question.Id;

        await _context.Options.AddAsync(newOption);

        await _context.SaveChangesAsync();

        return new Result() { Errors = [], Model = _mapper.Map<OptionDto>(newOption), Success = true };
    }

    public async Task<Result> Delete(string optionId)
    {
        List<Error> errors = [];

        var existingOption = await _context.Options.Where(i => i.UniqueId.Equals(optionId)).FirstOrDefaultAsync();

        if (existingOption is null) { errors.Add(new Error() { Message = $"Apologizes for incontinence! but we have not found any option with Id: {optionId}. Please enter a valid record to update, thanks." }); }

        if (errors.Any()) { return new Result() { Success = false, Errors = errors, Model = null }; }

        _context.Options.Remove(existingOption);

        await _context.SaveChangesAsync();

        return new Result() { Success = true, Errors = errors, Model = null };
    }
}
