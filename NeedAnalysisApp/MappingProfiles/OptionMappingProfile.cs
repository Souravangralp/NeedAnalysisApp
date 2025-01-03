﻿using AutoMapper;
using NeedAnalysisApp.Data.Models.Assessment;
using NeedAnalysisApp.Shared.Dto;

namespace NeedAnalysisApp.MappingProfiles;

public class OptionMappingProfile : Profile
{
    public OptionMappingProfile()
    {
        CreateMap<Option, OptionDto>()
            .ReverseMap()
            .ForMember(dest => dest.UniqueId, opt => opt.MapFrom(src => Guid.NewGuid().ToString())); 
    }
}
