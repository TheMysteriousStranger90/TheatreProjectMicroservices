using TheatreProject.PerformanceAPI.Models;
using TheatreProject.PerformanceAPI.Models.DTOs;

namespace TheatreProject.PerformanceAPI.Repositories;

public interface IPerformanceRepository
{
    Task<IEnumerable<PerformanceDto>> GetPerformances();
    Task<IEnumerable<PerformanceDto>> GetUpcomingPerformances();
    Task<PerformanceDto> GetPerformanceById(Guid id);
    Task<PerformanceDto> CreateUpdatePerformance(CreatePerformanceDto performanceDto);
    Task<PerformanceDto> UpdatePerformance(EditPerformanceDto dto, string baseUrl);
    Task<bool> DeletePerformance(Guid id);
    Task<PagedResponse<PerformanceDto>> GetFilteredPerformances(PerformanceQueryParameters parameters);
    Task<PerformanceStatistics> GetPerformanceStatistics(Guid id);
    Task<bool> UpdatePerformanceStatus(Guid id, PerformanceStatus status);
    Task<bool> IsPerformanceSoldOut(Guid id);
    Task<bool> UpdatePerformanceSeats(Guid performanceId, int bookedSeats);
}