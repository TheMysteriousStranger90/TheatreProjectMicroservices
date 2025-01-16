using TheatreProject.OrderAPI.Models.Enums;

namespace TheatreProject.OrderAPI.Models.DTOs;

public class PerformanceDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string TheatreName { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public int Capacity { get; set; }
    public decimal BasePrice { get; set; }
    public DateTime ShowDateTime { get; set; }
    public TimeSpan Duration { get; set; }
    public TheatreCategory Category { get; set; }
    public int AvailableSeats { get; set; }
    public string? ImageUrl { get; set; }
    public PerformanceStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int TotalBookings { get; set; }
    public decimal Revenue { get; set; }
    
    public int Count { get; set; } = 1;
}