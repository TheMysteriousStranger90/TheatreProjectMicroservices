﻿namespace TheatreProject.PerformanceAPI.Models;

public class PerformanceQueryParameters
{
    public string? SearchTerm { get; set; }
    public TheatreCategory? Category { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 6;
    public string SortBy { get; set; } = "date";
    public bool IsDescending { get; set; }
}