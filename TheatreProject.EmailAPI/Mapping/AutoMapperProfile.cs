using AutoMapper;
using TheatreProject.EmailAPI.Models;
using TheatreProject.EmailAPI.Models.DTOs;

namespace TheatreProject.EmailAPI.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<OrderConfirmationDto, EmailLogger>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.CustomerEmail))
            .ForMember(dest => dest.Message, opt =>
                opt.MapFrom(src => $"Order confirmation sent for Order #{src.OrderId}"))
            .ForMember(dest => dest.EmailSent, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}