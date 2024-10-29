using AutoMapper;
using NeedAnalysisApp.Data.Models;
using NeedAnalysisApp.Shared.Dto;

namespace NeedAnalysisApp.MappingProfiles;

public class ApplicationUserMappingProfile : Profile
{
    public ApplicationUserMappingProfile()
    {
        CreateMap<ApplicationUser, UserDto>()
            .ReverseMap();
    }
}
