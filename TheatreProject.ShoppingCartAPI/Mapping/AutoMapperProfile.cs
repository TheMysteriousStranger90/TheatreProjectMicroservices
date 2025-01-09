using AutoMapper;
using TheatreProject.ShoppingCartAPI.Models;
using TheatreProject.ShoppingCartAPI.Models.DTOs;

namespace TheatreProject.ShoppingCartAPI.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<PerformanceDto, Performance>().ReverseMap();
        CreateMap<CartHeader, CartHeaderDto>().ReverseMap()
            .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
        CreateMap<CartDetails, CartDetailsDto>().ReverseMap()
            .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
        CreateMap<Cart, CartDto>().ReverseMap();
    }
}