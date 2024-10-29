using AutoMapper;
using NeedAnalysisApp.Data.Models.Assessment;
using NeedAnalysisApp.Shared.Dto;

namespace NeedAnalysisApp.MappingProfiles;

public class QuestionMappingProfile : Profile
{
    public QuestionMappingProfile()
    {
        CreateMap<Question, QuestionDto>()
           .ReverseMap()
           .ForMember(dest => dest.UniqueId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));
    }
}
