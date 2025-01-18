using TheatreProject.WebApp.Models.DTOs;

namespace TheatreProject.WebApp.Services.Interfaces;

public interface IEmailService
{
    Task<T> SendOrderConfirmationAsync<T>(OrderConfirmationDto orderDetails, string token = null);
    Task<T> GetEmailLogsAsync<T>(string token = null);
}