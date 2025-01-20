namespace TheatreProject.WebApp.Models.DTOs;

public class PerformanceStatisticsDto
{
    public int TotalBookings { get; set; }
    public decimal TotalRevenue { get; set; }
    public int AvailableSeats { get; set; }
    public decimal OccupancyRate { get; set; }
}