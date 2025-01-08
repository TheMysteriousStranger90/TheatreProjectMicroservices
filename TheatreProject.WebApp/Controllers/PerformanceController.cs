using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TheatreProject.WebApp.Models;
using TheatreProject.WebApp.Services.Interfaces;

namespace TheatreProject.WebApp.Controllers;

public class PerformanceController : Controller
{
    private readonly IPerformanceService _performanceService;
    private readonly ILogger<PerformanceController> _logger;

    public PerformanceController(IPerformanceService performanceService, ILogger<PerformanceController> logger)
    {
        _performanceService = performanceService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        List<PerformanceDto> performances = new();
        var response = await _performanceService.GetPerformancesAsync<ResponseDto>("");

        if (response != null && response.IsSuccess)
        {
            performances = JsonConvert.DeserializeObject<List<PerformanceDto>>(Convert.ToString(response.Result));
        }
        return View(performances);
    }
    
    public IActionResult CreatePerformance()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreatePerformance([FromForm] CreatePerformanceDto model, IFormFile? Image)
    {
        try
        {
            _logger.LogInformation("Starting CreatePerformance with data: {@Model}", model);
        
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            if (string.IsNullOrEmpty(accessToken))
            {
                _logger.LogWarning("Access token is null or empty");
                return RedirectToAction("Login", "Auth");
            }

            model.Image = Image;
            var response = await _performanceService.CreatePerformanceAsync<ResponseDto>(model, accessToken);

            if (response != null)
            {
                _logger.LogInformation("API Response: {@Response}", response);
            
                if (response.IsSuccess)
                {
                    TempData["Success"] = "Performance created successfully";
                    return RedirectToAction(nameof(Search));
                }
            
                ModelState.AddModelError("", response.ErrorMessages?.FirstOrDefault() ?? "Unknown error");
            }
            else
            {
                _logger.LogWarning("API returned null response");
                ModelState.AddModelError("", "Server returned no response");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating performance");
            ModelState.AddModelError("", $"Error: {ex.Message}");
        }
    
        return View(model);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            _logger.LogInformation("Requesting performance details for ID: {Id}", id);

            var response = await _performanceService.GetPerformanceByIdAsync<ResponseDto>(id, accessToken);
            _logger.LogInformation("Response received: {@Response}", response);

            if (response?.IsSuccess == true && response.Result != null)
            {
                var model = JsonConvert.DeserializeObject<PerformanceDto>(
                    JsonConvert.SerializeObject(response.Result));
                
                if (model == null)
                {
                    _logger.LogWarning("Failed to deserialize performance data");
                    return NotFound();
                }
            
                return View(model);
            }

            _logger.LogWarning("Performance not found or unsuccessful response");
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving performance details for ID: {Id}", id);
            return NotFound();
        }
    }

    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var response = await _performanceService.GetPerformanceByIdAsync<ResponseDto>(id, accessToken);

        if (response != null && response.IsSuccess)
        {
            var model = JsonConvert.DeserializeObject<PerformanceDto>(Convert.ToString(response.Result));
            return View(model);
        }
        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Edit(PerformanceDto model)
    {
        if (ModelState.IsValid)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _performanceService.UpdatePerformanceAsync<ResponseDto>(model, accessToken);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(Search));
            }
            ModelState.AddModelError("", response?.ErrorMessages?.FirstOrDefault() ?? "Error occurred");
        }
        return View(model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _performanceService.DeletePerformanceAsync<ResponseDto>(id, accessToken);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(Search));
            }
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting performance {Id}", id);
            return Json(new { success = false, message = "Error deleting performance" });
        }
    }

    public async Task<IActionResult> Search([FromQuery] PerformanceQueryParameters parameters)
    {
        try
        {
            parameters ??= new PerformanceQueryParameters();
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            _logger.LogInformation("Requesting filtered performances with token: {Token}", accessToken);

            var response = await _performanceService.GetFilteredPerformancesAsync<ResponseDto>(parameters, accessToken);
            _logger.LogInformation("Response received: {@Response}", response);

            if (response?.IsSuccess == true && response.Result != null)
            {
                var result = JsonConvert.DeserializeObject<PagedResponse<PerformanceDto>>(
                    JsonConvert.SerializeObject(response.Result));
                return View(result);
            }

            _logger.LogWarning("No results or unsuccessful response");
            return View(new PagedResponse<PerformanceDto>
            {
                Data = new List<PerformanceDto>(),
                PageNumber = parameters?.PageNumber ?? 1,
                PageSize = parameters?.PageSize ?? 10
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Search action");
            return View(new PagedResponse<PerformanceDto>
            {
                Data = new List<PerformanceDto>(),
                PageNumber = parameters?.PageNumber ?? 1,
                PageSize = parameters?.PageSize ?? 10
            });
        }
    }

    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Statistics(Guid id)
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var response = await _performanceService.GetPerformanceStatisticsAsync<ResponseDto>(id, accessToken);

        if (response != null && response.IsSuccess)
        {
            var stats = JsonConvert.DeserializeObject<PerformanceStatisticsDto>(Convert.ToString(response.Result));
            return View(stats);
        }
        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateStatus(Guid id, PerformanceStatus status)
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var response = await _performanceService.UpdatePerformanceStatusAsync<ResponseDto>(id, status, accessToken);
        return Json(new { success = response?.IsSuccess ?? false, message = response?.DisplayMessage });
    }

    [HttpGet]
    public async Task<IActionResult> CheckAvailability(Guid id)
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var response = await _performanceService.CheckIfSoldOutAsync<ResponseDto>(id, accessToken);
        return Json(new { isSoldOut = response?.IsSuccess ?? false });
    }
}