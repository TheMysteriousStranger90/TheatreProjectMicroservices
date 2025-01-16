using TheatreProject.OrderAPI.Models;
using TheatreProject.OrderAPI.Models.DTOs;

namespace TheatreProject.OrderAPI.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<OrderHeader>> GetOrders(string userId);
    Task<OrderHeader> GetOrder(Guid orderId);
    Task<OrderHeader> CreateOrder(CartDto cartDto);
    Task<StripeRequestDto> CreatePaymentSession(StripeRequestDto stripeRequestDto);
    Task<OrderHeader> ValidatePaymentSession(Guid orderHeaderId);
    Task<bool> UpdateOrderStatus(Guid orderId, string newStatus);
}