﻿using AutoMapper;
using NeedAnalysisApp.Data.Models.Assessment;
using NeedAnalysisApp.Data.Models.Scores;
using NeedAnalysisApp.Shared.Dto;

namespace NeedAnalysisApp.MappingProfiles;

public class ScoreCategoryMappingProfile : Profile
{
    public ScoreCategoryMappingProfile()
    {
        CreateMap<ScoreCategory, ScoreCategoryDto>()
          .ReverseMap()
          .ForMember(dest => dest.UniqueId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));
    }
}
