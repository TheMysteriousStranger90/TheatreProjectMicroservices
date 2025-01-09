using System.Text;
using Newtonsoft.Json;
using TheatreProject.ShoppingCartAPI.Models;
using TheatreProject.ShoppingCartAPI.Models.DTOs;
using TheatreProject.ShoppingCartAPI.Services.Interfaces;

namespace TheatreProject.ShoppingCartAPI.Services;

public class CouponService : ICouponService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<CouponService> _logger;

    public CouponService(IHttpClientFactory clientFactory, ILogger<CouponService> logger)
    {
        _httpClientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<CouponDto> GetCoupon(string couponCode)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("CouponAPI");
            var response = await client.GetAsync($"/api/coupon/GetByCode/{couponCode}");
            
            response.EnsureSuccessStatusCode();
            
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            
            if (resp?.IsSuccess == true)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result));
            }
            
            _logger.LogWarning("Coupon not found: {CouponCode}", couponCode);
            return new CouponDto();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving coupon: {CouponCode}", couponCode);
            throw;
        }
    }
    
    public async Task<bool> DoesCouponExist(string couponCode)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("CouponAPI");
            var response = await client.GetAsync($"/api/coupon/exists/{couponCode}");
        
            response.EnsureSuccessStatusCode();
        
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
        
            if (resp?.IsSuccess == true)
            {
                return JsonConvert.DeserializeObject<bool>(Convert.ToString(resp.Result));
            }
        
            _logger.LogWarning("Coupon not found: {CouponCode}", couponCode);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking coupon existence: {CouponCode}", couponCode);
            return false;
        }
    }
    
    public async Task<bool> ValidateCoupon(string couponCode, decimal orderAmount, int ticketCount)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("CouponAPI");
            var response = await client.GetAsync($"/api/coupon/validate?code={couponCode}&amount={orderAmount}&tickets={ticketCount}");
        
            response.EnsureSuccessStatusCode();
        
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
        
            if (resp?.IsSuccess == true)
            {
                return JsonConvert.DeserializeObject<bool>(Convert.ToString(resp.Result));
            }
        
            _logger.LogWarning("Coupon validation failed: {CouponCode}", couponCode);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating coupon: {CouponCode}", couponCode);
            return false;
        }
    }
    
    public async Task<decimal> CalculateDiscount(string couponCode, decimal amount)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("CouponAPI");
            var response = await client.GetAsync($"/api/coupon/calculate?code={couponCode}&amount={amount}");
        
            response.EnsureSuccessStatusCode();
        
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
        
            if (resp?.IsSuccess == true)
            {
                return JsonConvert.DeserializeObject<decimal>(Convert.ToString(resp.Result));
            }
        
            _logger.LogWarning("Discount calculation failed: {CouponCode}", couponCode);
            return 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating discount: {CouponCode}", couponCode);
            return 0;
        }
    }

    public async Task<CouponValidationResult> ValidateCouponForCart(string couponCode, CartDto cart)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("CouponAPI");
            var content = new StringContent(
                JsonConvert.SerializeObject(new { couponCode, cart }),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/api/coupon/validate-cart", content);

            response.EnsureSuccessStatusCode();

            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if (resp?.IsSuccess == true)
            {
                return JsonConvert.DeserializeObject<CouponValidationResult>(Convert.ToString(resp.Result));
            }

            _logger.LogWarning("Cart validation failed for coupon: {CouponCode}", couponCode);
            return new CouponValidationResult
            {
                IsValid = false,
                Message = "Validation failed",
                ValidationErrors = new List<string> { resp?.DisplayMessage ?? "Unknown error" }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating cart with coupon: {CouponCode}", couponCode);
            return new CouponValidationResult
            {
                IsValid = false,
                Message = "Service error",
                ValidationErrors = new List<string> { "Service unavailable" }
            };
        }
    }
}