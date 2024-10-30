using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NeedAnalysisApp.Repositories.Interfaces;
using NeedAnalysisApp.Shared.Common;

namespace NeedAnalysisApp.Controllers;


[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UsersController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet("api/users")]
    public async Task<Result> GetAll(string? role)
    {
        var test = new Result()
        {
            Success = true,
            Model = await _userService.GetAll(role),
            Errors = []
        };

        return test;
    }

    [HttpGet("api/users/{uniqueId}")]
    public async Task<Result> GetWithId(string uniqueId)
    {
        return new()
        {
            Success = true,
            Model = await _userService.GetWithId(uniqueId),
            Errors = []
        };
    }
}
