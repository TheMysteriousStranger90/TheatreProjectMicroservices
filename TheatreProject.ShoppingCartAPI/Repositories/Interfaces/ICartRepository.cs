using TheatreProject.ShoppingCartAPI.Models.DTOs;
using TheatreProject.ShoppingCartAPI.Models.Enums;

namespace TheatreProject.ShoppingCartAPI.Repositories.Interfaces;

public interface ICartRepository
{
    Task<CartDto> GetCartByUserId(string userId);
    Task<CartDto> CreateUpdateCart(CartDto cartDto);
    Task<bool> RemoveFromCart(Guid cartDetailsId);
    Task<bool> ApplyCoupon(string userId, string couponCode);
    Task<bool> RemoveCoupon(string userId);
    Task<bool> ClearCart(string userId);
    
    
    Task<bool> ValidateCart(CartDto cartDto);
    Task<CartDetailsDto> GetCartDetail(Guid cartDetailId);
    Task<bool> UpdateQuantity(Guid cartDetailId, int quantity);
    Task<bool> ValidateSeats(Guid performanceId, string seats);
    Task<decimal> CalculateTotal(string userId);
    Task<bool> IsPerformanceAvailable(Guid performanceId, int quantity);
    Task<bool> LockSeats(Guid performanceId, string seats, TimeSpan duration);
    Task<CartStatus> GetCartStatus(string userId);
    Task<bool> SaveCartForLater(string userId);
}