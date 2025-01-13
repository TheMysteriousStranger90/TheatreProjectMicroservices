using TheatreProject.WebApp.Models.DTOs;

namespace TheatreProject.WebApp.Services.Interfaces;

public interface ICartService
{
    Task<T> GetCartByUserIdAsync<T>(string userId, string token = null);
    Task<T> AddToCartAsync<T>(CartDto cartDto, string token = null);
    Task<T> UpdateCartAsync<T>(CartDto cartDto, string token = null);
    Task<T> RemoveFromCartAsync<T>(Guid cartId, string token = null);
    Task<T> ClearCartAsync<T>(string userId, string token = null);
    Task<T> ApplyCoupon<T>(CartDto cartDto, string token = null);
    Task<T> RemoveCoupon<T>(string userId, string token = null);
    
    
    Task<T> ValidateCartAsync<T>(string userId, string token = null);
    Task<T> UpdateQuantityAsync<T>(Guid cartDetailId, int quantity, string token = null);
    Task<T> ValidateSeatsAsync<T>(Guid performanceId, string seats, string token = null);
    Task<T> CalculateTotalAsync<T>(string userId, string token = null);
    Task<T> GetCartStatusAsync<T>(string userId, string token = null);
    Task<T> SaveCartForLaterAsync<T>(string userId, string token = null);
    Task<T> GetCartDetailAsync<T>(Guid cartDetailId, string token = null);
    Task<T> ValidateCouponForCartAsync<T>(CartDto cartDto, string token = null);
}