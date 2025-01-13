﻿using TheatreProject.WebApp.Constants;
using TheatreProject.WebApp.Models.DTOs;
using TheatreProject.WebApp.Models.Enums;
using TheatreProject.WebApp.Services.Interfaces;

namespace TheatreProject.WebApp.Services;

public class CartService : ICartService
{
    private readonly IBaseService _baseService;
    private readonly ILogger<CartService> _logger;

    public CartService(IBaseService baseService, ILogger<CartService> logger)
    {
        _baseService = baseService;
        _logger = logger;
    }

    public async Task<T> GetCartByUserIdAsync<T>(string userId, string token = null)
    {
        return await _baseService.SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.GET,
            Url = $"{Const.ShoppingCartAPIBase}/api/cart/GetCart/{userId}",
            AccessToken = token
        });
    }

    public async Task<T> AddToCartAsync<T>(CartDto cartDto, string token = null)
    {
        return await _baseService.SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.POST,
            Data = cartDto,
            Url = $"{Const.ShoppingCartAPIBase}/api/cart/AddCart",
            AccessToken = token
        });
    }

    public async Task<T> UpdateCartAsync<T>(CartDto cartDto, string token = null)
    {
        return await _baseService.SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.PUT,
            Data = cartDto,
            Url = $"{Const.ShoppingCartAPIBase}/api/cart/UpdateCart",
            AccessToken = token
        });
    }

    public async Task<T> RemoveFromCartAsync<T>(Guid cartId, string token = null)
    {
        return await _baseService.SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.DELETE,
            Data = cartId,
            Url = $"{Const.ShoppingCartAPIBase}/api/cart/RemoveCart",
            AccessToken = token
        });
    }

    public async Task<T> ClearCartAsync<T>(string userId, string token = null)
    {
        return await _baseService.SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.DELETE,
            Url = $"{Const.ShoppingCartAPIBase}/api/cart/ClearCart/{userId}",
            AccessToken = token
        });
    }

    public async Task<T> ApplyCoupon<T>(CartDto cartDto, string token = null)
    {
        return await _baseService.SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.POST,
            Data = cartDto,
            Url = $"{Const.ShoppingCartAPIBase}/api/cart/ApplyCoupon",
            AccessToken = token
        });
    }

    public async Task<T> RemoveCoupon<T>(string userId, string token = null)
    {
        return await _baseService.SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.POST,
            Data = userId,
            Url = $"{Const.ShoppingCartAPIBase}/api/cart/RemoveCoupon",
            AccessToken = token
        });
    }
    
    
    
    
    public async Task<T> ValidateCartAsync<T>(string userId, string token = null)
    {
        return await _baseService.SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.GET,
            Url = $"{Const.ShoppingCartAPIBase}/api/cart/ValidateCart?userId={userId}",
            AccessToken = token
        });
    }

    public async Task<T> UpdateQuantityAsync<T>(Guid cartDetailId, int quantity, string token = null)
    {
        return await _baseService.SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.PUT,
            Data = quantity,
            Url = $"{Const.ShoppingCartAPIBase}/api/cart/UpdateQuantity/{cartDetailId}",
            AccessToken = token
        });
    }
    
    public async Task<T> ValidateSeatsAsync<T>(Guid performanceId, string seats, string token = null)
    {
        return await _baseService.SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.GET,
            Url = $"{Const.ShoppingCartAPIBase}/api/cart/ValidateSeats?performanceId={performanceId}&seats={seats}",
            AccessToken = token
        });
    }

    public async Task<T> CalculateTotalAsync<T>(string userId, string token = null)
    {
        return await _baseService.SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.GET,
            Url = $"{Const.ShoppingCartAPIBase}/api/cart/CalculateTotal/{userId}",
            AccessToken = token
        });
    }
    
    public async Task<T> GetCartStatusAsync<T>(string userId, string token = null)
    {
        return await _baseService.SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.GET,
            Url = $"{Const.ShoppingCartAPIBase}/api/cart/Status/{userId}",
            AccessToken = token
        });
    }

    public async Task<T> SaveCartForLaterAsync<T>(string userId, string token = null)
    {
        return await _baseService.SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.POST,
            Url = $"{Const.ShoppingCartAPIBase}/api/cart/SaveForLater/{userId}",
            AccessToken = token
        });
    }
    
    public async Task<T> GetCartDetailAsync<T>(Guid cartDetailId, string token = null)
    {
        return await _baseService.SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.GET,
            Url = $"{Const.ShoppingCartAPIBase}/api/cart/Detail/{cartDetailId}",
            AccessToken = token
        });
    }

    public async Task<T> ValidateCouponForCartAsync<T>(CartDto cartDto, string token = null)
    {
        return await _baseService.SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.POST,
            Data = cartDto,
            Url = $"{Const.ShoppingCartAPIBase}/api/cart/ValidateCouponForCart",
            AccessToken = token
        });
    }
}