using Newtonsoft.Json;
using TheatreProject.OrderAPI.Models.DTOs;
using TheatreProject.OrderAPI.Services.Interfaces;

namespace TheatreProject.OrderAPI.Services;

public class PerformanceService : IPerformanceService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<PerformanceService> _logger;

    public PerformanceService(IHttpClientFactory clientFactory, ILogger<PerformanceService> logger)
    {
        _httpClientFactory = clientFactory;
        _logger = logger;
    }


    public async Task<IEnumerable<PerformanceDto>> GetPerformances()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("PerformanceAPI");
            var response = await client.GetAsync($"/api/performances");
            
            response.EnsureSuccessStatusCode();
            
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            
            if (resp?.IsSuccess == true)
            {
                return JsonConvert.DeserializeObject<IEnumerable<PerformanceDto>>(Convert.ToString(resp.Result));
            }
            
            return new List<PerformanceDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving performances");
            throw;
        }
    }
}