using TheatreProject.ShoppingCartAPI.Models;
using TheatreProject.ShoppingCartAPI.Models.DTOs;

namespace TheatreProject.ShoppingCartAPI.Repositories.Interfaces;

public interface ICouponRepository
{
    Task<CouponDto> GetCoupon(string couponName);
    Task<bool> DoesCouponExist(string couponCode);
    
    Task<bool> ValidateCoupon(string couponCode, decimal orderAmount, int ticketCount);
    Task<decimal> CalculateDiscount(string couponCode, decimal amount);
    Task<CouponValidationResult> ValidateCouponForCart(string couponCode, CartDto cart);
}