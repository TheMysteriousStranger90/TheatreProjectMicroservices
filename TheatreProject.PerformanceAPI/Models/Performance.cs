using System.ComponentModel.DataAnnotations;

namespace TheatreProject.PerformanceAPI.Models;

public class Performance
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string TheatreName { get; set; }
    [Required]
    public string City { get; set; }
    [Required]
    public string Address { get; set; }
    public int Capacity { get; set; }
    [Required]
    [Range(1, 1000000)]
    public decimal BasePrice { get; set; }
    public DateTime ShowDateTime { get; set; }
    public TimeSpan Duration { get; set; }
    public TheatreCategory Category { get; set; }
    public int AvailableSeats { get; set; }
    public int TotalBookings { get; set; }
    public decimal Revenue { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageLocalPath { get; set; }
    public PerformanceStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}