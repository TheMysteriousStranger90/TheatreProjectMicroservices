using TheatreProject.OrderAPI.Models.DTOs;

namespace TheatreProject.OrderAPI.Services.Interfaces;

public interface IPerformanceService
{
    Task<IEnumerable<PerformanceDto>> GetPerformances();
    Task<bool> UpdatePerformanceSeats(Guid performanceId, int bookedSeats);
}