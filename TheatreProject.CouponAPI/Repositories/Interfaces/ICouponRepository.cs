using TheatreProject.CouponAPI.Models;
using TheatreProject.CouponAPI.Models.DTOs;

namespace TheatreProject.CouponAPI.Repositories.Interfaces;

public interface ICouponRepository
{
    Task<CouponDto> GetCouponByCode(string couponCode);
    Task<bool> DoesCouponExist(string couponCode);
    Task<IEnumerable<CouponDto>> GetAllCoupons();
    Task<CouponDto> GetCouponById(Guid id);
    Task<ResponseDto> CreateCoupon(CouponDto couponDto);
    Task<ResponseDto> UpdateCoupon(CouponDto couponDto);
    Task<ResponseDto> DeleteCoupon(Guid id);
    
    Task<bool> ValidateCoupon(string couponCode, decimal orderAmount, int ticketCount);
    Task<decimal> CalculateDiscount(string couponCode, decimal amount);
    Task<CouponValidationResult> ValidateCouponForCart(string couponCode, CartDto cart);
}