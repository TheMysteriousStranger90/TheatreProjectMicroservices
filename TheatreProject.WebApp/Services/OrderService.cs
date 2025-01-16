using TheatreProject.WebApp.Constants;
using TheatreProject.WebApp.Models.DTOs;
using TheatreProject.WebApp.Models.Enums;
using TheatreProject.WebApp.Services.Interfaces;

namespace TheatreProject.WebApp.Services;

public class OrderService : IOrderService
{
    private readonly IBaseService _baseService;
    private readonly ILogger<OrderService> _logger;

    public OrderService(IBaseService baseService, ILogger<OrderService> logger)
    {
        _baseService = baseService;
        _logger = logger;
    }

    public async Task<T> GetOrdersAsync<T>(string userId, string token = null)
    {
        return await _baseService.SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.GET,
            Url = $"{Const.OrderAPIBase}/api/order/GetOrders?userId={userId}",
            AccessToken = token
        });
    }

    public async Task<T> GetOrderAsync<T>(Guid orderId, string token = null)
    {
        return await _baseService.SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.GET,
            Url = $"{Const.OrderAPIBase}/api/order/GetOrder/{orderId}",
            AccessToken = token
        });
    }

    public async Task<T> CreateOrderAsync<T>(CartDto cartDto, string token = null)
    {
        return await _baseService.SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.POST,
            Data = cartDto,
            Url = $"{Const.OrderAPIBase}/api/order/CreateOrder",
            AccessToken = token
        });
    }

    public async Task<T> CreatePaymentSessionAsync<T>(StripeRequestDto stripeRequestDto, string token = null)
    {
        return await _baseService.SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.POST,
            Data = stripeRequestDto,
            Url = $"{Const.OrderAPIBase}/api/order/CreatePaymentSession",
            AccessToken = token
        });
    }

    public async Task<T> ValidatePaymentAsync<T>(Guid orderId, string token = null)
    {
        return await _baseService.SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.POST,
            Url = $"{Const.OrderAPIBase}/api/order/ValidatePayment/{orderId}",
            AccessToken = token
        });
    }

    public async Task<T> UpdateOrderStatusAsync<T>(Guid orderId, string newStatus, string token = null)
    {
        return await _baseService.SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.POST,
            Data = newStatus,
            Url = $"{Const.OrderAPIBase}/api/order/UpdateOrderStatus/{orderId}",
            AccessToken = token
        });
    }
}