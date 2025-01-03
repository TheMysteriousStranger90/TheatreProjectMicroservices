using AutoMapper;
using TheatreProject.PerformanceAPI.Models;
using TheatreProject.PerformanceAPI.Models.Dto;

namespace TheatreProject.PerformanceAPI.Mapping;

public class AutoMapperProfile  : Profile
{
    public AutoMapperProfile ()
    {
        CreateMap<Performance, PerformanceDto>().ReverseMap();
    }
}