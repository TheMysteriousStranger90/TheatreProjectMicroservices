namespace TheatreProject.WebApp.Models;

public class PerformanceStatisticsDto
{
    public int TotalBookings { get; set; }
    public decimal TotalRevenue { get; set; }
    public int AvailableSeats { get; set; }
    public double OccupancyRate { get; set; }
}