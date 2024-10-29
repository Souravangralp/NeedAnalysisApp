using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NeedAnalysisApp.Data.Models;
using NeedAnalysisApp.Repositories.Interfaces;
using NeedAnalysisApp.Shared.Dto;

namespace NeedAnalysisApp.Repositories.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public UserService(
        UserManager<ApplicationUser> userManager,
        IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<List<UserDto>> GetAll(string? role)
    {
        var users = string.IsNullOrWhiteSpace(role) 
            ? await _userManager.Users.ToListAsync()
            : await _userManager.GetUsersInRoleAsync("User");

        return _mapper.Map<List<UserDto>>(users.ToList());
    }

    public async Task<UserDto> GetWithId(string id)
    {
       var user = await _userManager.FindByIdAsync(id) ?? new ApplicationUser();

        return _mapper.Map<UserDto>(user);
    }
}
