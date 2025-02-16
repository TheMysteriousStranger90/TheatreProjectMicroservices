﻿using TheatreProject.WebApp.Models;
using TheatreProject.WebApp.Models.DTOs;

namespace TheatreProject.WebApp.Services.Interfaces;

public interface IPerformanceService
{
    Task<T> GetPerformancesAsync<T>(string token);
    Task<T> GetPerformanceByIdAsync<T>(Guid id, string token);
    Task<T> GetUpcomingPerformancesAsync<T>(string token);
    Task<T> CreatePerformanceAsync<T>(CreatePerformanceDto performanceDto, string token);
    Task<T> UpdatePerformanceAsync<T>(EditPerformanceDto performanceDto, string token);
    Task<T> DeletePerformanceAsync<T>(Guid id, string token);
    Task<T> GetFilteredPerformancesAsync<T>(PerformanceQueryParameters parameters, string token);
    Task<T> GetPerformanceStatisticsAsync<T>(Guid id, string token);
    Task<T> UpdatePerformanceStatusAsync<T>(Guid id, PerformanceStatus status, string token);
    Task<T> CheckIfSoldOutAsync<T>(Guid id, string token);
}