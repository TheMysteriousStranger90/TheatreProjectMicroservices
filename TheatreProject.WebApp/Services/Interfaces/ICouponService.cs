using TheatreProject.WebApp.Models.DTOs;

namespace TheatreProject.WebApp.Services.Interfaces;

public interface ICouponService
{
    Task<T> GetCoupon<T>(string couponCode, string token = null);
    Task<T> GetAllCoupons<T>(string token = null);
    Task<T> CreateCoupon<T>(CouponDto couponDto, string token = null);
    Task<T> DeleteCoupon<T>(Guid id, string token = null);
}