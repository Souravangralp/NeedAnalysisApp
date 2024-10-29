using AutoMapper;
using NeedAnalysisApp.Data.Models.Industries;
using NeedAnalysisApp.Shared.Dto;

namespace NeedAnalysisApp.MappingProfiles;

public class IndustryMappingProfile : Profile
{
    public IndustryMappingProfile()
    {
        CreateMap<Industry, IndustryDto>()
            .ReverseMap()
            .ForMember(dest => dest.UniqueId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));
        
    }
}
