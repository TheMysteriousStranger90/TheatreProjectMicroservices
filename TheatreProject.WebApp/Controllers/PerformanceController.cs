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
        var response = await _performanceService.GetPerformancesAsync();
        if (response != null && response.IsSuccess)
        {
            var list = JsonConvert.DeserializeObject<List<PerformanceDto>>(Convert.ToString(response.Result));
            return View(list);
        }

        return View(new List<PerformanceDto>());
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var response = await _performanceService.GetPerformanceByIdAsync(id);
        if (response != null && response.IsSuccess)
        {
            var performance = JsonConvert.DeserializeObject<PerformanceDto>(Convert.ToString(response.Result));
            return View(performance);
        }

        return NotFound();
    }

    public async Task<IActionResult> Upcoming()
    {
        var response = await _performanceService.GetUpcomingPerformancesAsync();
        if (response != null && response.IsSuccess)
        {
            var list = JsonConvert.DeserializeObject<List<PerformanceDto>>(Convert.ToString(response.Result));
            return View(list);
        }

        return View(new List<PerformanceDto>());
    }

    [Authorize(Roles = "Administrator")]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize(Roles = "Administrator")]
    [HttpPost]
    public async Task<IActionResult> Create(PerformanceDto model)
    {
        if (ModelState.IsValid)
        {
            var response = await _performanceService.CreatePerformanceAsync(model);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", response?.Message ?? "Error occurred");
        }

        return View(model);
    }

    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var response = await _performanceService.GetPerformanceByIdAsync(id);
        if (response != null && response.IsSuccess)
        {
            var performance = JsonConvert.DeserializeObject<PerformanceDto>(Convert.ToString(response.Result));
            return View(performance);
        }

        return NotFound();
    }

    [Authorize(Roles = "Administrator")]
    [HttpPost]
    public async Task<IActionResult> Edit(PerformanceDto model)
    {
        if (ModelState.IsValid)
        {
            var response = await _performanceService.UpdatePerformanceAsync(model);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", response?.Message ?? "Error occurred");
        }

        return View(model);
    }

    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _performanceService.DeletePerformanceAsync(id);
        if (response != null && response.IsSuccess)
        {
            TempData["Success"] = "Performance deleted successfully";
        }
        else
        {
            TempData["Error"] = response?.Message ?? "Error occurred";
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Search([FromQuery] PerformanceQueryParameters parameters)
    {
        parameters ??= new PerformanceQueryParameters();
        var response = await _performanceService.GetFilteredPerformancesAsync(parameters);
    
        var result = response?.IsSuccess == true 
            ? JsonConvert.DeserializeObject<PagedResponse<PerformanceDto>>(Convert.ToString(response.Result))
            : new PagedResponse<PerformanceDto> 
            { 
                Data = new List<PerformanceDto>(),
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };

        return View(result);
    }
    
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Statistics(Guid id)
    {
        var response = await _performanceService.GetPerformanceStatisticsAsync(id);
        if (response != null && response.IsSuccess)
        {
            var stats = JsonConvert.DeserializeObject<PerformanceStatisticsDto>(Convert.ToString(response.Result));
            return View(stats);
        }

        return NotFound();
    }

    [Authorize(Roles = "Administrator")]
    [HttpPost]
    public async Task<IActionResult> UpdateStatus(Guid id, PerformanceStatus status)
    {
        var response = await _performanceService.UpdatePerformanceStatusAsync(id, status);
        return Json(new { success = response?.IsSuccess ?? false, message = response?.Message });
    }

    [HttpGet]
    public async Task<IActionResult> CheckAvailability(Guid id)
    {
        var response = await _performanceService.CheckIfSoldOutAsync(id);
        return Json(new { isSoldOut = Convert.ToBoolean(response?.Result) });
    }
}