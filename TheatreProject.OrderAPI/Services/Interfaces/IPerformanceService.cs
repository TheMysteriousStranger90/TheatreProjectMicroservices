using TheatreProject.OrderAPI.Models.DTOs;

namespace TheatreProject.OrderAPI.Services.Interfaces;

public interface IPerformanceService
{
    Task<IEnumerable<PerformanceDto>> GetPerformances();
}