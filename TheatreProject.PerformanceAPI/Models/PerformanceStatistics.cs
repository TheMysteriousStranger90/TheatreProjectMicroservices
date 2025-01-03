namespace TheatreProject.PerformanceAPI.Models;

public class PerformanceStatistics
{
    public int TotalBookings { get; set; }
    public decimal TotalRevenue { get; set; }
    public int AvailableSeats { get; set; }
    public double OccupancyRate { get; set; }
}