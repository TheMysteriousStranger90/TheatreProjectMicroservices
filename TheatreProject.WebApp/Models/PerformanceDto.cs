using TheatreProject.WebApp.Utility;

namespace TheatreProject.WebApp.Models;

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
    public string ImageUrl { get; set; }
    
    public PerformanceStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int TotalBookings { get; set; }
    public decimal Revenue { get; set; }
    
    
    
    [MaxFileSize(1)]
    [AllowedExtensions(new string[] { ".jpg", ".png" })]
    public IFormFile? Image { get; set; }
}