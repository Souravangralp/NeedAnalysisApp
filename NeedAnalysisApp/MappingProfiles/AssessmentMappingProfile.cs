using AutoMapper;
using NeedAnalysisApp.Data.Models.Assessment;
using NeedAnalysisApp.Shared.Dto;

namespace NeedAnalysisApp.MappingProfiles;

public class AssessmentMappingProfile : Profile
{
    public AssessmentMappingProfile()
    {
        CreateMap<Assessment, AssessmentDto>()
            .ReverseMap()
            .ForMember(dest => dest.UniqueId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));
        
    }
}
