using AutoMapper;
using Newtonsoft.Json;
using TheatreProject.ShoppingCartAPI.Data;
using TheatreProject.ShoppingCartAPI.Models;
using TheatreProject.ShoppingCartAPI.Models.DTOs;
using TheatreProject.ShoppingCartAPI.Repositories.Interfaces;

namespace TheatreProject.ShoppingCartAPI.Repositories;

public class CouponRepository : ICouponRepository
{
    private readonly HttpClient _client;

    public CouponRepository(HttpClient client)
    {
        _client = client;
    }

    public async Task<CouponDto> GetCoupon(string couponName)
    {
        var response = await _client.GetAsync($"/api/coupon/{couponName}");
        var apiContent = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

        if (resp.IsSuccess)
        {
            return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result));
        }

        return new CouponDto();
    }

    public async Task<bool> DoesCouponExist(string couponName)
    {
        var response = await _client.GetAsync($"/api/coupon/exist/{couponName}");
        var apiContent = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

        if (resp.IsSuccess)
            return (bool)resp.Result;

        return false;
    }

    public async Task<bool> ValidateCoupon(string couponCode, decimal orderAmount, int ticketCount)
    {
        var response =
            await _client.GetAsync(
                $"/api/coupon/validate/{couponCode}?orderAmount={orderAmount}&ticketCount={ticketCount}");
        var apiContent = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

        if (resp.IsSuccess)
            return (bool)resp.Result;

        return false;
    }

    public async Task<decimal> CalculateDiscount(string couponCode, decimal amount)
    {
        var response = await _client.GetAsync($"/api/coupon/calculate/{couponCode}?amount={amount}");
        var apiContent = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

        if (resp.IsSuccess)
            return Convert.ToDecimal(resp.Result);

        return 0;
    }

    public async Task<CouponValidationResult> ValidateCouponForCart(string couponCode, CartDto cart)
    {
        var requestData = new
        {
            CouponCode = couponCode,
            Cart = cart
        };

        var response = await _client.PostAsJsonAsync("/api/coupon/validate-cart", requestData);
        var apiContent = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

        if (resp.IsSuccess)
            return JsonConvert.DeserializeObject<CouponValidationResult>(Convert.ToString(resp.Result));

        return new CouponValidationResult
        {
            IsValid = false,
            Message = "Error validating coupon",
            ValidationErrors = new List<string> { "Unable to process coupon validation" }
        };
    }
}