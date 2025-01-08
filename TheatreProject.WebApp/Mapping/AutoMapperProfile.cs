using AutoMapper;
using TheatreProject.WebApp.Models.DTOs;

namespace TheatreProject.WebApp.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<PerformanceDto, EditPerformanceDto>();
    }
}