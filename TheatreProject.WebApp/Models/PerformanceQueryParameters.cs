using System.ComponentModel.DataAnnotations;
using TheatreProject.WebApp.Models.Enums;

namespace TheatreProject.WebApp.Models;

public class PerformanceQueryParameters
{
    public string SearchTerm { get; set; }
    public TheatreCategory? Category { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    [Range(0, double.MaxValue)]
    public decimal? MinPrice { get; set; }
    [Range(0, double.MaxValue)]
    public decimal? MaxPrice { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 6;
    public string SortBy { get; set; } = "date";
    public bool IsDescending { get; set; }
}