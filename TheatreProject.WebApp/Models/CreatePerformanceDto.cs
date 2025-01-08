using System.ComponentModel.DataAnnotations;
using TheatreProject.WebApp.Utility;

namespace TheatreProject.WebApp.Models;

public class CreatePerformanceDto
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Description is required")]
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
    
    [Required(ErrorMessage = "Available Seats is required")]
    [Range(1, 10000, ErrorMessage = "Available Seats must be between 1 and 10000")]
    public int AvailableSeats { get; set; }
    
    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, 1000000, ErrorMessage = "Price must be between 0.01 and 1000000")]
    public decimal BasePrice { get; set; }
    
    [Required(ErrorMessage = "Show date and time is required")]
    public DateTime ShowDateTime { get; set; }
    
    [Required(ErrorMessage = "Duration is required")]
    public TimeSpan Duration { get; set; }
    
    [Required(ErrorMessage = "Category is required")]
    public TheatreCategory Category { get; set; }
    
    [MaxFileSize(1)]
    [AllowedExtensions(new string[] { ".jpg", ".png" })]
    public IFormFile? Image { get; set; }
}