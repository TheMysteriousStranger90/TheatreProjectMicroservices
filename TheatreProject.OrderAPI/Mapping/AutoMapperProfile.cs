using AutoMapper;
using TheatreProject.OrderAPI.Models;
using TheatreProject.OrderAPI.Models.DTOs;

namespace TheatreProject.OrderAPI.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<OrderHeaderDto, CartHeaderDto>()
            .ForMember(dest=>dest.GrandTotal, u=>u.MapFrom(src=>src.GrandTotal)).ReverseMap();

        CreateMap<CartDetailsDto, OrderDetailsDto>()
            .ForMember(dest => dest.PerformanceName, u => u.MapFrom(src => src.PerformanceName))
            .ForMember(dest => dest.SubTotal, u => u.MapFrom(src => src.SubTotal));
        
        CreateMap<OrderDetailsDto, CartDetailsDto>();
        CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
        CreateMap<OrderDetailsDto, OrderDetails>().ReverseMap();
    }
}