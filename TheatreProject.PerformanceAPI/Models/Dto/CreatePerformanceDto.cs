using System.ComponentModel.DataAnnotations;

namespace TheatreProject.PerformanceAPI.Models.Dto;

public class CreatePerformanceDto
{
    [Required] public string Name { get; set; }
    [Required] public string Description { get; set; }

    [Required] public string TheatreName { get; set; }

    [Required] public string City { get; set; }

    [Required] public string Address { get; set; }
    [Required] [Range(1, 10000)] public int Capacity { get; set; }

    [Required] [Range(1, 10000)] public int AvailableSeats { get; set; }
    [Required] [Range(0.01, 1000000)] public decimal BasePrice { get; set; }
    [Required] public DateTime ShowDateTime { get; set; }
    [Required] public TimeSpan Duration { get; set; }
    [Required] public TheatreCategory Category { get; set; }

    public string? ImageUrl { get; set; }
    public string? ImageLocalPath { get; set; }
    public IFormFile? Image { get; set; }
}