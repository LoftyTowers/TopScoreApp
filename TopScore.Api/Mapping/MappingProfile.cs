using AutoMapper;
using TopScore.Core.Models;
using TopScore.Api.Models; // We'll add this next

namespace TopScore.Api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<WordEntry, WordEntryModel>().ReverseMap();
    }
}
