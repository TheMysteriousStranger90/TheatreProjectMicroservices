using TheatreProject.WebApp.Models;

namespace TheatreProject.WebApp.Services.Interfaces;

public interface IPerformanceService : IBaseService
{
    Task<T> GetPerformancesAsync<T>(string token);
    Task<T> GetPerformanceByIdAsync<T>(Guid id, string token);
    Task<T> GetUpcomingPerformancesAsync<T>(string token);
    Task<T> CreatePerformanceAsync<T>(PerformanceDto performanceDto, string token);
    Task<T> UpdatePerformanceAsync<T>(PerformanceDto performanceDto, string token);
    Task<T> DeletePerformanceAsync<T>(Guid id, string token);
    Task<T> GetFilteredPerformancesAsync<T>(PerformanceQueryParameters parameters, string token);
    Task<T> GetPerformanceStatisticsAsync<T>(Guid id, string token);
    Task<T> UpdatePerformanceStatusAsync<T>(Guid id, PerformanceStatus status, string token);
    Task<T> CheckIfSoldOutAsync<T>(Guid id, string token);
}