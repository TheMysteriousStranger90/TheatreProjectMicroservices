using TheatreProject.WebApp.Models;

namespace TheatreProject.WebApp.Services.Interfaces;

public interface IPerformanceService
{
    Task<ResponseDto?> GetPerformancesAsync();
    Task<ResponseDto?> GetPerformanceByIdAsync(Guid id);
    Task<ResponseDto?> GetUpcomingPerformancesAsync();
    Task<ResponseDto?> CreatePerformanceAsync(PerformanceDto performanceDto);
    Task<ResponseDto?> UpdatePerformanceAsync(PerformanceDto performanceDto);
    Task<ResponseDto?> DeletePerformanceAsync(Guid id);
    Task<ResponseDto?> GetFilteredPerformancesAsync(PerformanceQueryParameters parameters);
    Task<ResponseDto?> GetPerformanceStatisticsAsync(Guid id);
    Task<ResponseDto?> UpdatePerformanceStatusAsync(Guid id, PerformanceStatus status);
    Task<ResponseDto?> CheckIfSoldOutAsync(Guid id);
}