using TheatreProject.EmailAPI.Models.DTOs;

namespace TheatreProject.EmailAPI.Services.Interfaces;

public interface IEmailService
{
    Task SendOrderConfirmationAsync(OrderConfirmationDto orderDetails);
}