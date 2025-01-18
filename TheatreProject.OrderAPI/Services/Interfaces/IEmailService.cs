using TheatreProject.OrderAPI.Models.DTOs;

namespace TheatreProject.OrderAPI.Services.Interfaces;

public interface IEmailService
{
    Task<bool> SendOrderConfirmationAsync(OrderConfirmationDto orderDetails);
}