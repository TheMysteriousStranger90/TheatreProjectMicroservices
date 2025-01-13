namespace TheatreProject.WebApp.Services.Interfaces;

public interface ICouponService
{
    Task<T> GetCoupon<T>(string couponCode, string token = null);
}