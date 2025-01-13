using AutoMapper;
using TheatreProject.CouponAPI.Models;
using TheatreProject.CouponAPI.Models.DTOs;

namespace TheatreProject.CouponAPI.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Coupon, CouponDto>().ReverseMap();
    }
}