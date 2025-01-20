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
                Url = $"{Const.ServerAPIBase}/api/coupon/GetByCode/{couponCode}",
                AccessToken = token
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting coupon: {CouponCode}", couponCode);
            throw;
        }
    }
    
    public async Task<T> GetAllCoupons<T>(string token = null)
    {
        try
        {
            _logger.LogInformation("Getting all coupons");
            return await _baseService.SendAsync<T>(new RequestDto
            {
                ApiType = ApiType.GET,
                Url = $"{Const.ServerAPIBase}/api/coupon",
                AccessToken = token
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all coupons");
            throw;
        }
    }
    
    public async Task<T> CreateCoupon<T>(CouponDto couponDto, string token = null)
    {
        try
        {
            _logger.LogInformation("Creating new coupon");
            return await _baseService.SendAsync<T>(new RequestDto
            {
                ApiType = ApiType.POST,
                Data = couponDto,
                Url = $"{Const.ServerAPIBase}/api/coupon",
                AccessToken = token
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating coupon");
            throw;
        }
    }
    
    public async Task<T> DeleteCoupon<T>(Guid id, string token = null)
    {
        try
        {
            _logger.LogInformation("Deleting coupon: {Id}", id);
            return await _baseService.SendAsync<T>(new RequestDto
            {
                ApiType = ApiType.DELETE,
                Url = $"{Const.ServerAPIBase}/api/coupon/{id}",
                AccessToken = token
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting coupon: {Id}", id);
            throw;
        }
    }
}