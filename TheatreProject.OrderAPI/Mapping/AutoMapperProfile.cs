using AutoMapper;
using TheatreProject.OrderAPI.Models;
using TheatreProject.OrderAPI.Models.DTOs;

namespace TheatreProject.OrderAPI.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        
        /*
        CreateMap<OrderHeaderDto, CartHeaderDto>()
            .ForMember(dest=>dest.GrandTotal, u=>u.MapFrom(src=>src.GrandTotal)).ReverseMap();

        CreateMap<CartDetailsDto, OrderDetailsDto>()
            .ForMember(dest => dest.PerformanceName, u => u.MapFrom(src => src.PerformanceName))
            .ForMember(dest => dest.SubTotal, u => u.MapFrom(src => src.SubTotal));
        
        CreateMap<OrderDetailsDto, CartDetailsDto>();
        CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
        CreateMap<OrderDetailsDto, OrderDetails>().ReverseMap();
        */
        
        
        CreateMap<CartDto, OrderHeader>()
            .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.CartDetails))
            .ForMember(dest => dest.OrderTime, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.CartHeader.UserId))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.CartHeader.Email))
            .ForMember(dest => dest.GrandTotal, opt => opt.MapFrom(src => src.CartHeader.GrandTotal))
            .ForMember(dest => dest.DiscountTotal, opt => opt.MapFrom(src => src.CartHeader.DiscountTotal))
            .ForMember(dest => dest.CouponCode, opt => opt.MapFrom(src => src.CartHeader.CouponCode));

        CreateMap<CartDetailsDto, OrderDetails>()
            .ForMember(dest => dest.PerformanceName, opt => opt.MapFrom(src => src.PerformanceName))
            .ForMember(dest => dest.PricePerTicket, opt => opt.MapFrom(src => src.PricePerTicket))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.SeatNumbers, opt => opt.MapFrom(src => src.SeatNumbers))
            .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.SubTotal))
            .ForMember(dest => dest.TicketType, opt => opt.MapFrom(src => src.TicketType))
            .ForMember(dest => dest.PerformanceId, opt => opt.MapFrom(src => src.PerformanceId));

        // Existing mappings
        CreateMap<OrderHeaderDto, CartHeaderDto>().ReverseMap();
        CreateMap<CartDetailsDto, OrderDetailsDto>().ReverseMap();
        CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
        CreateMap<OrderDetails, OrderDetailsDto>().ReverseMap();
    }
}