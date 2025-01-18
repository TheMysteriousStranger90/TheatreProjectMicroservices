using TheatreProject.OrderAPI.Models.DTOs;
using TheatreProject.OrderAPI.Services.Interfaces;

namespace TheatreProject.OrderAPI.Services;
public class EmailService : IEmailService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<EmailService> _logger;
    private const int MaxRetries = 3;

    public EmailService(IHttpClientFactory httpClientFactory, ILogger<EmailService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("EmailAPI");
        _logger = logger;
    }

    public async Task<bool> SendOrderConfirmationAsync(OrderConfirmationDto orderDetails)
    {
        for (int i = 0; i < MaxRetries; i++)
        {
            try
            {
                _logger.LogInformation("Attempt {Attempt} to send email confirmation", i + 1);
                var response = await _httpClient.PostAsJsonAsync("api/email/SendOrderConfirmation", orderDetails);
                
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Email sent successfully");
                    return true;
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Email service responded with {StatusCode}: {Error}", 
                    response.StatusCode, errorContent);
                
                if (i < MaxRetries - 1)
                    await Task.Delay(1000 * (i + 1));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending confirmation email (Attempt {Attempt})", i + 1);
                if (i < MaxRetries - 1)
                    await Task.Delay(1000 * (i + 1));
            }
        }

        return false;
    }
}