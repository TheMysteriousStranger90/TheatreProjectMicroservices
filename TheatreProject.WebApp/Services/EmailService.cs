using TheatreProject.WebApp.Constants;
using TheatreProject.WebApp.Models.DTOs;
using TheatreProject.WebApp.Models.Enums;
using TheatreProject.WebApp.Services.Interfaces;

namespace TheatreProject.WebApp.Services;

public class EmailService : BaseService, IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(IHttpClientFactory httpClientFactory, ILogger<EmailService> logger) 
        : base(httpClientFactory, logger)
    {
        _logger = logger;
    }

    public async Task<T> SendOrderConfirmationAsync<T>(OrderConfirmationDto orderDetails, string token = null)
    {
        return await SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.POST,
            Data = orderDetails,
            Url = $"{Const.ServerAPIBase}/api/email/SendOrderConfirmation",
            AccessToken = token
        });
    }

    public async Task<T> GetEmailLogsAsync<T>(string token = null)
    {
        return await SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.GET,
            Url = $"{Const.ServerAPIBase}/api/email/logs",
            AccessToken = token
        });
    }
}