namespace TheatreProject.WebApp.Models.DTOs;

public class PerformanceStatisticsDto
{
    public int TotalBookings { get; set; }
    public decimal TotalRevenue { get; set; }
    public int AvailableSeats { get; set; }
    private decimal _occupancyRate;
    public decimal OccupancyRate 
    { 
        get => _occupancyRate;
        set => _occupancyRate = Math.Round(value, 1);
    }
}