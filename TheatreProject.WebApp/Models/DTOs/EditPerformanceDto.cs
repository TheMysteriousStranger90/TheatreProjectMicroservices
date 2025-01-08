using System.ComponentModel.DataAnnotations;
using TheatreProject.WebApp.Models.Enums;
using TheatreProject.WebApp.Utility;

namespace TheatreProject.WebApp.Models.DTOs;

public class EditPerformanceDto
{
    [Required]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Description is required")]
    [MaxLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string Description { get; set; }
    
    [Required(ErrorMessage = "Theatre name is required")]
    public string TheatreName { get; set; }
    
    [Required(ErrorMessage = "City is required")]
    public string City { get; set; }
    
    [Required(ErrorMessage = "Address is required")]
    public string Address { get; set; }
    
    [Required(ErrorMessage = "Capacity is required")]
    [Range(1, 10000, ErrorMessage = "Capacity must be between 1 and 10000")]
    public int Capacity { get; set; }
    
    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, 1000000, ErrorMessage = "Price must be between 0.01 and 1000000")]
    public decimal BasePrice { get; set; }
    
    [Required(ErrorMessage = "Show date and time is required")]
    public DateTime ShowDateTime { get; set; }
    
    [Required(ErrorMessage = "Duration is required")]
    public TimeSpan Duration { get; set; }
    
    [Required(ErrorMessage = "Category is required")]
    public TheatreCategory Category { get; set; }

    [Required(ErrorMessage = "Status is required")]
    public PerformanceStatus Status { get; set; }
    
    [Required(ErrorMessage = "Available seats is required")]
    [Range(0, 10000, ErrorMessage = "Available seats must be between 0 and 10000")]
    public int AvailableSeats { get; set; }

    public string? ImageUrl { get; set; }
    
    [MaxFileSize(1)]
    [AllowedExtensions(new string[] { ".jpg", ".png" })]
    public IFormFile? Image { get; set; }
}