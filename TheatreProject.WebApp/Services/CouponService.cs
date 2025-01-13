using TheatreProject.WebApp.Constants;
using TheatreProject.WebApp.Models.DTOs;
using TheatreProject.WebApp.Models.Enums;
using TheatreProject.WebApp.Services.Interfaces;

namespace TheatreProject.WebApp.Services;

public class CouponService : ICouponService
{
    private readonly IBaseService _baseService;
    private readonly ILogger<CouponService> _logger;

    public CouponService(IBaseService baseService, ILogger<CouponService> logger)
    {
        _baseService = baseService;
        _logger = logger;
    }

    public async Task<T> GetCoupon<T>(string couponCode, string token = null)
    {
        try
        {
            _logger.LogInformation("Getting coupon with code: {CouponCode}", couponCode);
        
            return await _baseService.SendAsync<T>(new RequestDto
            {
                ApiType = ApiType.GET,
                Url = $"{Const.CouponAPIBase}/api/coupon/GetByCode/{couponCode}",
                AccessToken = token
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting coupon: {CouponCode}", couponCode);
            throw;
        }
    }
}