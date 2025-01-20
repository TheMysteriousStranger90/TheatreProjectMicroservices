using AutoMapper;
using TheatreProject.OrderAPI.Models;
using TheatreProject.OrderAPI.Models.DTOs;

namespace TheatreProject.OrderAPI.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<OrderHeader, OrderHeaderDto>()
            .ForMember(dest => dest.OrderDetails,
                opt => opt.MapFrom(src => src.OrderDetails));

        CreateMap<OrderHeaderDto, OrderHeader>();

        CreateMap<OrderDetails, OrderDetailsDto>()
            .ReverseMap();

        CreateMap<CartDto, OrderHeader>()
            .ForMember(dest => dest.OrderDetails,
                opt => opt.MapFrom(src => src.CartDetails))
            .ForMember(dest => dest.OrderTime,
                opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.UserId,
                opt => opt.MapFrom(src => src.CartHeader.UserId))
            .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.CartHeader.Email))
            .ForMember(dest => dest.GrandTotal,
                opt => opt.MapFrom(src => src.CartHeader.GrandTotal))
            .ForMember(dest => dest.DiscountTotal,
                opt => opt.MapFrom(src => src.CartHeader.DiscountTotal))
            .ForMember(dest => dest.CouponCode,
                opt => opt.MapFrom(src => src.CartHeader.CouponCode));

        CreateMap<CartDetailsDto, OrderDetails>()
            .ForMember(dest => dest.PerformanceName,
                opt => opt.MapFrom(src => src.Performance.Name))
            .ForMember(dest => dest.PricePerTicket,
                opt => opt.MapFrom(src => src.PricePerTicket))
            .ForMember(dest => dest.Quantity,
                opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.SeatNumbers,
                opt => opt.MapFrom(src => src.SeatNumbers))
            .ForMember(dest => dest.SubTotal,
                opt => opt.MapFrom(src => src.SubTotal))
            .ForMember(dest => dest.TicketType,
                opt => opt.MapFrom(src => src.TicketType))
            .ForMember(dest => dest.PerformanceId,
                opt => opt.MapFrom(src => src.PerformanceId));

        CreateMap<OrderHeaderDto, CartHeaderDto>().ReverseMap();
        CreateMap<CartDetailsDto, OrderDetailsDto>().ReverseMap();
        CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
        CreateMap<OrderDetails, OrderDetailsDto>().ReverseMap();


        CreateMap<OrderHeader, OrderConfirmationDto>()
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.CustomerName, opt =>
                opt.MapFrom(src => $"{src.FirstName} {src.LastName}".Trim()))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.GrandTotal))
            .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderTime))
            .ForMember(dest => dest.OrderDetails, opt =>
                opt.MapFrom(src => src.OrderDetails));

        CreateMap<OrderDetails, OrderDetailsDto>()
            .ForMember(dest => dest.PerformanceName, opt =>
                opt.MapFrom(src => src.PerformanceName))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.PricePerTicket, opt =>
                opt.MapFrom(src => src.PricePerTicket))
            .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.SubTotal))
            .ForMember(dest => dest.SeatNumbers, opt =>
                opt.MapFrom(src => src.SeatNumbers));
    }
}