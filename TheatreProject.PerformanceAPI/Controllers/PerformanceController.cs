using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TheatreProject.PerformanceAPI.Models;
using TheatreProject.PerformanceAPI.Models.DTOs;
using TheatreProject.PerformanceAPI.Repositories;
using TheatreProject.PerformanceAPI.Services;
using TheatreProject.PerformanceAPI.Validators;

namespace TheatreProject.PerformanceAPI.Controllers;

[Route("api/performances")]
[ApiController]
[ServiceFilter(typeof(ValidationFilter))]
public class PerformanceController : ControllerBase
{
    private const string PerformancesCacheKey = "Performances";
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly IPerformanceRepository _repository;
    private readonly ILogger<PerformanceController> _logger;
    private readonly IMemoryCache _memoryCache;
    private readonly ICacheKeyService _cacheKeyService;
    private ResponseDto _response;

    public PerformanceController(
        IPerformanceRepository repository,
        ILogger<PerformanceController> logger,
        IMemoryCache memoryCache,
        ICacheKeyService cacheKeyService)
    {
        _repository = repository;
        _logger = logger;
        _memoryCache = memoryCache;
        _cacheKeyService = cacheKeyService;
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
    public async Task<ActionResult<ResponseDto>> CreatePerformance([FromForm] CreatePerformanceDto performanceDto)
    {
        try
        {
            var validator = new PerformanceDtoValidator();
            var validationResult = await validator.ValidateAsync(performanceDto);

            if (!validationResult.IsValid)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(_response);
            }

            if (performanceDto.Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(performanceDto.Image.FileName);
                string relativePath = "PerformanceImages/" + fileName;
                string absolutePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

                Directory.CreateDirectory(Path.GetDirectoryName(absolutePath));

                using (var fileStream = new FileStream(absolutePath, FileMode.Create))
                {
                    await performanceDto.Image.CopyToAsync(fileStream);
                }

                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}";
                performanceDto.ImageUrl = $"{baseUrl}/{relativePath}";
                performanceDto.ImageLocalPath = "wwwroot/" + relativePath;
            }

            var result = await _repository.CreateUpdatePerformance(performanceDto);
            _response.Result = result;

            var keysToRemove = _cacheKeyService.GetKeysStartingWith(PerformancesCacheKey);
            foreach (var cacheKey in keysToRemove)
            {
                _logger.LogDebug("Removing cache key after create: {CacheKey}", cacheKey);
                _memoryCache.Remove(cacheKey);
                _cacheKeyService.RemoveKey(cacheKey);
            }

            return _response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating performance");
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
            return StatusCode(500, _response);
        }
    }
    
    [HttpPut]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<ResponseDto>> UpdatePerformance([FromForm] EditPerformanceDto dto)
    {
        try
        {
            _logger.LogInformation("Updating performance: {Id}", dto.Id);

            var validator = new EditPerformanceDtoValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for performance update: {Id}", dto.Id);
                _response.IsSuccess = false;
                _response.ErrorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(_response);
            }

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
            var updatedPerformance = await _repository.UpdatePerformance(dto, baseUrl);
    
            if (updatedPerformance == null)
            {
                _logger.LogWarning("Performance not found: {Id}", dto.Id);
                return NotFound(_response);
            }

            _response.Result = updatedPerformance;
        
            var keysToRemove = _cacheKeyService.GetKeysStartingWith(PerformancesCacheKey);
            foreach (var cacheKey in keysToRemove)
            {
                _logger.LogDebug("Removing cache key after update: {CacheKey}", cacheKey);
                _memoryCache.Remove(cacheKey);
                _cacheKeyService.RemoveKey(cacheKey);
            }

            _logger.LogInformation("Successfully updated performance: {Id}", updatedPerformance.Id);
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating performance: {Id}", dto.Id);
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
            return StatusCode(500, _response);
        }
    }

    [Authorize(Roles = "Administrator")]
    [HttpDelete("{id}")]
    public async Task<object> DeletePerformance(Guid id)
    {
        try
        {
            var isSuccess = await _repository.DeletePerformance(id);
            _response.Result = isSuccess;

            if (isSuccess)
            {
                var keysToRemove = _cacheKeyService.GetKeysStartingWith(PerformancesCacheKey);
                foreach (var cacheKey in keysToRemove)
                {
                    _logger.LogDebug("Removing cache key after delete: {CacheKey}", cacheKey);
                    _memoryCache.Remove(cacheKey);
                    _cacheKeyService.RemoveKey(cacheKey);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting performance: {Id}", id);
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

            string cacheKey = $"{PerformancesCacheKey}_filtered_{parameters.GetHashCode()}";

            if (!_memoryCache.TryGetValue(cacheKey, out PagedResponse<PerformanceDto> performances))
            {
                try
                {
                    await semaphore.WaitAsync();
                    if (!_memoryCache.TryGetValue(cacheKey, out performances))
                    {
                        performances = await _repository.GetFilteredPerformances(parameters);

                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                            .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                        _memoryCache.Set(cacheKey, performances, cacheEntryOptions);
                        _cacheKeyService.AddKey(cacheKey);
                    }
                }
                finally
                {
                    semaphore.Release();
                }
            }

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
            var cacheKey = $"{PerformancesCacheKey}_stats_{id}";

            if (!_memoryCache.TryGetValue(cacheKey, out var statistics))
            {
                try
                {
                    await semaphore.WaitAsync();
                    if (!_memoryCache.TryGetValue(cacheKey, out statistics))
                    {
                        _logger.LogInformation("Cache miss for statistics, fetching from database");
                        statistics = await _repository.GetPerformanceStatistics(id);

                        var cacheOptions = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                            .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                        _memoryCache.Set(cacheKey, statistics, cacheOptions);
                        _cacheKeyService.AddKey(cacheKey);
                    }
                }
                finally
                {
                    semaphore.Release();
                }
            }

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
                var cacheKey = $"{PerformancesCacheKey}_stats_{id}";
                _memoryCache.Remove(cacheKey);
                _cacheKeyService.RemoveKey(cacheKey);
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
            var cacheKey = $"{PerformancesCacheKey}_soldout_{id}";

            if (!_memoryCache.TryGetValue(cacheKey, out bool isSoldOut))
            {
                try
                {
                    await semaphore.WaitAsync();
                    if (!_memoryCache.TryGetValue(cacheKey, out isSoldOut))
                    {
                        isSoldOut = await _repository.IsPerformanceSoldOut(id);

                        var cacheOptions = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

                        _memoryCache.Set(cacheKey, isSoldOut, cacheOptions);
                        _cacheKeyService.AddKey(cacheKey);
                    }
                }
                finally
                {
                    semaphore.Release();
                }
            }

            _response.Result = isSoldOut;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return _response;
    }
    
    [HttpPut("{id}/seats")]
    public async Task<ActionResult<ResponseDto>> UpdatePerformanceSeats(Guid id, [FromQuery] int bookedSeats)
    {
        try
        {
            var success = await _repository.UpdatePerformanceSeats(id, bookedSeats);
            if (!success)
                return BadRequest(new ResponseDto { IsSuccess = false, DisplayMessage = "Failed to update seats" });

            return Ok(new ResponseDto { IsSuccess = true });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResponseDto 
            { 
                IsSuccess = false, 
                ErrorMessages = new List<string> { ex.Message } 
            });
        }
    }
}