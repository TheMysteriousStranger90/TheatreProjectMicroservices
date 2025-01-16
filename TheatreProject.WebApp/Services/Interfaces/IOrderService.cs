using TheatreProject.WebApp.Models.DTOs;

namespace TheatreProject.WebApp.Services.Interfaces;

public interface IOrderService
{
    Task<T> GetOrdersAsync<T>(string userId, string token = null);
    Task<T> GetOrderAsync<T>(Guid orderId, string token = null);
    Task<T> CreateOrderAsync<T>(CartDto cartDto, string token = null);
    Task<T> CreatePaymentSessionAsync<T>(StripeRequestDto stripeRequestDto, string token = null);
    Task<T> ValidatePaymentAsync<T>(Guid orderId, string token = null);
    Task<T> UpdateOrderStatusAsync<T>(Guid orderId, string status, string token = null);
}