using TheatreProject.ShoppingCartAPI.Models;
using TheatreProject.ShoppingCartAPI.Models.DTOs;

namespace TheatreProject.ShoppingCartAPI.Services.Interfaces;

public interface ICouponService
{
    Task<CouponDto> GetCoupon(string couponCode);
    Task<bool> DoesCouponExist(string couponName);
    Task<bool> ValidateCoupon(string couponCode, decimal orderAmount, int ticketCount);
    Task<decimal> CalculateDiscount(string couponCode, decimal amount);
    Task<CouponValidationResult> ValidateCouponForCart(string couponCode, CartDto cart);
}