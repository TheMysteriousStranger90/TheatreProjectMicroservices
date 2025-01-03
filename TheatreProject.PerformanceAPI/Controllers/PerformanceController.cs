using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheatreProject.PerformanceAPI.Models;
using TheatreProject.PerformanceAPI.Models.Dto;
using TheatreProject.PerformanceAPI.Repositories;
using TheatreProject.PerformanceAPI.Services;
using TheatreProject.PerformanceAPI.Validators;

namespace TheatreProject.PerformanceAPI.Controllers;

[Route("api/performances")]
[ApiController]
[ServiceFilter(typeof(ValidationFilter))]
public class PerformanceController : ControllerBase
{
    private ResponseDto _response;
    private readonly IPerformanceRepository _repository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<PerformanceController> _logger;

    public PerformanceController(
        IPerformanceRepository repository,
        ICacheService cacheService,
        ILogger<PerformanceController> logger)
    {
        _repository = repository;
        _cacheService = cacheService;
        _logger = logger;
        _response = new ResponseDto();
    }

    [HttpGet]
    public async Task<object> GetPerformances()
    {
        try
        {
            _logger.LogInformation("Getting all performances");
            IEnumerable<PerformanceDto> performances = await _repository.GetPerformances();
            _response.Result = performances;
            _logger.LogInformation("Successfully retrieved {Count} performances", performances.Count());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all performances");
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return _response;
    }

    [HttpGet("upcoming")]
    public async Task<object> GetUpcomingPerformances()
    {
        try
        {
            _logger.LogInformation("Fetching upcoming performances");
            IEnumerable<PerformanceDto> performances = await _repository.GetUpcomingPerformances();
            _response.Result = performances;
            _logger.LogInformation("Retrieved {Count} upcoming performances", performances.Count());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving upcoming performances");
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return _response;
    }

    [HttpGet("{id}")]
    public async Task<object> GetPerformanceById(Guid id)
    {
        try
        {
            _logger.LogInformation("Fetching performance with ID: {Id}", id);
            PerformanceDto performance = await _repository.GetPerformanceById(id);
            if (performance == null)
            {
                _logger.LogWarning("Performance not found with ID: {Id}", id);
            }

            _response.Result = performance;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving performance with ID: {Id}", id);
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return _response;
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<object> CreatePerformance([FromBody] PerformanceDto performanceDto)
    {
        try
        {
            _logger.LogInformation("Creating new performance: {Name}", performanceDto.Name);
            var validator = new PerformanceDtoValidator();
            var validationResult = await validator.ValidateAsync(performanceDto);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for performance creation");
                _response.IsSuccess = false;
                _response.ErrorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return _response;
            }

            PerformanceDto performance = await _repository.CreateUpdatePerformance(performanceDto);
            _response.Result = performance;
            _logger.LogInformation("Successfully created performance with ID: {Id}", performance.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating performance");
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return _response;
    }


    [Authorize(Roles = "Administrator")]
    [HttpPut]
    public async Task<object> UpdatePerformance([FromBody] PerformanceDto performanceDto)
    {
        try
        {
            _logger.LogInformation("Updating performance: {Id}", performanceDto.Id);
            var validator = new PerformanceDtoValidator();
            var validationResult = await validator.ValidateAsync(performanceDto);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for performance update: {Id}", performanceDto.Id);
                _response.IsSuccess = false;
                _response.ErrorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return _response;
            }

            PerformanceDto performance = await _repository.CreateUpdatePerformance(performanceDto);
            _response.Result = performance;
            _logger.LogInformation("Successfully updated performance: {Id}", performance.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating performance: {Id}", performanceDto.Id);
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return _response;
    }

    [Authorize(Roles = "Administrator")]
    [HttpDelete("{id}")]
    public async Task<object> DeletePerformance(Guid id)
    {
        try
        {
            bool isSuccess = await _repository.DeletePerformance(id);
            _response.Result = isSuccess;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return _response;
    }

    [HttpGet("search")]
    public async Task<object> GetFilteredPerformances([FromQuery] PerformanceQueryParameters parameters)
    {
        try
        {
            parameters ??= new PerformanceQueryParameters();
            _logger.LogInformation("Searching performances with parameters: {@Parameters}", parameters);
        
            var cacheKey = $"performances_filtered_{parameters.GetHashCode()}";
            var performances = await _cacheService.GetOrSetAsync(cacheKey,
                async () =>
                {
                    _logger.LogDebug("Cache miss for filtered performances");
                    return await _repository.GetFilteredPerformances(parameters);
                },
                TimeSpan.FromMinutes(5));
            
            _logger.LogInformation("Retrieved {Count} filtered performances", performances.Data.Count());
            _response.Result = performances;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching performances");
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return _response;
    }

    [HttpGet("{id}/statistics")]
    public async Task<object> GetPerformanceStatistics(Guid id)
    {
        try
        {
            _logger.LogInformation("Getting statistics for performance ID: {Id}", id);
            var cacheKey = $"performance_stats_{id}";
            var statistics = await _cacheService.GetOrSetAsync(cacheKey,
                async () =>
                {
                    _logger.LogInformation("Cache miss for statistics, fetching from database");
                    return await _repository.GetPerformanceStatistics(id);
                },
                TimeSpan.FromMinutes(5));

            _response.Result = statistics;
            _logger.LogInformation("Successfully retrieved statistics for performance ID: {Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting statistics for performance ID: {Id}", id);
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return _response;
    }

    [Authorize(Roles = "Administrator")]
    [HttpPatch("{id}/status")]
    public async Task<object> UpdatePerformanceStatus(Guid id, [FromBody] PerformanceStatus status)
    {
        try
        {
            _logger.LogInformation("Updating status for performance {Id} to {Status}", id, status);
            bool result = await _repository.UpdatePerformanceStatus(id, status);
            if (result)
            {
                _logger.LogDebug("Removing cached statistics for performance {Id}", id);
                await _cacheService.RemoveAsync($"performance_stats_{id}");
                _logger.LogInformation("Successfully updated performance status");
            }
            else
            {
                _logger.LogWarning("Performance not found: {Id}", id);
            }

            _response.Result = result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating status for performance {Id}", id);
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return _response;
    }

    [HttpGet("{id}/sold-out")]
    public async Task<object> CheckIfSoldOut(Guid id)
    {
        try
        {
            var cacheKey = $"performance_soldout_{id}";
            var isSoldOut = await _cacheService.GetOrSetAsync(cacheKey,
                async () => await _repository.IsPerformanceSoldOut(id),
                TimeSpan.FromMinutes(1));
            _response.Result = isSoldOut;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return _response;
    }
}