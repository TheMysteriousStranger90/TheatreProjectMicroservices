using AutoMapper;
using TheatreProject.PerformanceAPI.Models;
using TheatreProject.PerformanceAPI.Models.DTOs;

namespace TheatreProject.PerformanceAPI.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Performance, PerformanceDto>().ReverseMap();

        CreateMap<CreatePerformanceDto, Performance>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => PerformanceStatus.Scheduled))
            .ForMember(dest => dest.AvailableSeats, opt => opt.MapFrom(src => src.Capacity))
            .ForMember(dest => dest.TotalBookings, opt => opt.Ignore())
            .ForMember(dest => dest.Revenue, opt => opt.Ignore())
            .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
            .ForMember(dest => dest.ImageLocalPath, opt => opt.Ignore());

        CreateMap<CreatePerformanceDto, PerformanceDto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => PerformanceStatus.Scheduled))
            .ForMember(dest => dest.AvailableSeats, opt => opt.MapFrom(src => src.Capacity))
            .ForMember(dest => dest.TotalBookings, opt => opt.Ignore())
            .ForMember(dest => dest.Revenue, opt => opt.Ignore())
            .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
        
        CreateMap<Performance, EditPerformanceDto>().ReverseMap()
            .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}