using Microsoft.AspNetCore.Http.Extensions;
using TheatreProject.WebApp.Constants;
using TheatreProject.WebApp.Models;
using TheatreProject.WebApp.Models.DTOs;
using TheatreProject.WebApp.Models.Enums;
using TheatreProject.WebApp.Services.Interfaces;

namespace TheatreProject.WebApp.Services;

public class PerformanceService : BaseService, IPerformanceService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<PerformanceService> _logger;

    public PerformanceService(IHttpClientFactory clientFactory, ILogger<PerformanceService> logger) : base(clientFactory, logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<T> GetPerformancesAsync<T>(string token)
    {
        return await SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.GET,
            Url = $"{Const.PerformanceAPIBase}/api/performances",
            AccessToken = token
        });
    }

    public async Task<T> GetPerformanceByIdAsync<T>(Guid id, string token)
    {
        return await SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.GET,
            Url = $"{Const.PerformanceAPIBase}/api/performances/{id}",
            AccessToken = token
        });
    }

    public async Task<T> GetUpcomingPerformancesAsync<T>(string token)
    {
        return await SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.GET,
            Url = $"{Const.PerformanceAPIBase}/api/performances/upcoming",
            AccessToken = token
        });
    }

    public async Task<T> CreatePerformanceAsync<T>(CreatePerformanceDto performanceDto, string token)
    {
        return await SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.POST,
            Data = performanceDto,
            Url = $"{Const.PerformanceAPIBase}/api/performances",
            AccessToken = token,
            ContentType = ContentType.MultipartFormData
        });
    }

    public async Task<T> UpdatePerformanceAsync<T>(EditPerformanceDto performanceDto, string token)
    {
        return await SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.PUT,
            Data = performanceDto,
            Url = $"{Const.PerformanceAPIBase}/api/performances",
            AccessToken = token,
            ContentType = ContentType.MultipartFormData
        });
    }

    public async Task<T> DeletePerformanceAsync<T>(Guid id, string token)
    {
        return await SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.DELETE,
            Url = $"{Const.PerformanceAPIBase}/api/performances/{id}",
            AccessToken = token
        });
    }

    public async Task<T> GetFilteredPerformancesAsync<T>(PerformanceQueryParameters parameters, string token)
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

        var queryString = new QueryBuilder(query).ToQueryString().Value;

        return await SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.GET,
            Url = $"{Const.PerformanceAPIBase}/api/performances/search{queryString}",
            AccessToken = token
        });
    }

    public async Task<T> GetPerformanceStatisticsAsync<T>(Guid id, string token)
    {
        return await SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.GET,
            Url = $"{Const.PerformanceAPIBase}/api/performances/{id}/statistics",
            AccessToken = token
        });
    }

    public async Task<T> UpdatePerformanceStatusAsync<T>(Guid id, PerformanceStatus status, string token)
    {
        return await SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.PUT,
            Data = status,
            Url = $"{Const.PerformanceAPIBase}/api/performances/{id}/status",
            AccessToken = token
        });
    }

    public async Task<T> CheckIfSoldOutAsync<T>(Guid id, string token)
    {
        return await SendAsync<T>(new RequestDto
        {
            ApiType = ApiType.GET,
            Url = $"{Const.PerformanceAPIBase}/api/performances/{id}/sold-out",
            AccessToken = token
        });
    }
}