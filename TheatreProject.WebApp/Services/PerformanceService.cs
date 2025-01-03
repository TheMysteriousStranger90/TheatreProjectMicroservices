using Microsoft.AspNetCore.Http.Extensions;
using TheatreProject.WebApp.Models;
using TheatreProject.WebApp.Services.Interfaces;

namespace TheatreProject.WebApp.Services;

public class PerformanceService : IPerformanceService
{
    private readonly IBaseService _baseService;
    private readonly string _performanceApiBase;

    public PerformanceService(IBaseService baseService, IConfiguration configuration)
    {
        _baseService = baseService;
        _performanceApiBase = configuration.GetValue<string>("ServiceUrls:PerformanceAPI");
    }

    public async Task<ResponseDto?> GetPerformancesAsync()
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.GET,
            Url = $"{_performanceApiBase}/api/performances"
        });
    }

    public async Task<ResponseDto?> GetPerformanceByIdAsync(Guid id)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.GET,
            Url = $"{_performanceApiBase}/api/performances/{id}"
        });
    }

    public async Task<ResponseDto?> GetUpcomingPerformancesAsync()
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.GET,
            Url = $"{_performanceApiBase}/api/performances/upcoming"
        });
    }

    public async Task<ResponseDto?> CreatePerformanceAsync(PerformanceDto performanceDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.POST,
            Data = performanceDto,
            Url = $"{_performanceApiBase}/api/performances",
            ContentType = ContentType.MultipartFormData
        });
    }

    public async Task<ResponseDto?> UpdatePerformanceAsync(PerformanceDto performanceDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.PUT,
            Data = performanceDto,
            Url = $"{_performanceApiBase}/api/performances",
            ContentType = ContentType.MultipartFormData
        });
    }

    public async Task<ResponseDto?> DeletePerformanceAsync(Guid id)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.DELETE,
            Url = $"{_performanceApiBase}/api/performances/{id}"
        });
    }

    public async Task<ResponseDto?> GetFilteredPerformancesAsync(PerformanceQueryParameters parameters)
    {
        var query = new Dictionary<string, string>();
    
        if (!string.IsNullOrEmpty(parameters.SearchTerm))
            query.Add("SearchTerm", parameters.SearchTerm);
    
        if (parameters.Category.HasValue)
            query.Add("Category", parameters.Category.ToString());
    
        if (parameters.StartDate.HasValue)
            query.Add("StartDate", parameters.StartDate.Value.ToString("o"));
    
        if (parameters.EndDate.HasValue)
            query.Add("EndDate", parameters.EndDate.Value.ToString("o"));
    
        query.Add("PageNumber", parameters.PageNumber.ToString());
        query.Add("PageSize", parameters.PageSize.ToString());
        query.Add("SortBy", parameters.SortBy ?? "date");
        query.Add("IsDescending", parameters.IsDescending.ToString());

        var queryString = new QueryBuilder(query).ToString();

        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.GET,
            Url = $"{_performanceApiBase}/api/performances/search{queryString}"
        });
    }

    public async Task<ResponseDto?> GetPerformanceStatisticsAsync(Guid id)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.GET,
            Url = $"{_performanceApiBase}/api/performances/{id}/statistics"
        });
    }

    public async Task<ResponseDto?> UpdatePerformanceStatusAsync(Guid id, PerformanceStatus status)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.PUT,
            Data = status,
            Url = $"{_performanceApiBase}/api/performances/{id}/status",
            ContentType = ContentType.MultipartFormData
        });
    }

    public async Task<ResponseDto?> CheckIfSoldOutAsync(Guid id)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.GET,
            Url = $"{_performanceApiBase}/api/performances/{id}/sold-out"
        });
    }
}