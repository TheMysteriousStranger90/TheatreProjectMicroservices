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

    [Authorize(Roles = "Administrator")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Create(PerformanceDto model)
    {
        if (ModelState.IsValid)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _performanceService.CreatePerformanceAsync<ResponseDto>(model, accessToken);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", response?.ErrorMessages?.FirstOrDefault() ?? "Error occurred");
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
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", response?.ErrorMessages?.FirstOrDefault() ?? "Error occurred");
        }
        return View(model);
    }

    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(Guid id)
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
    public async Task<IActionResult> Delete(PerformanceDto model)
    {
        if (ModelState.IsValid)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _performanceService.DeletePerformanceAsync<ResponseDto>(model.Id, accessToken);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
        }
        return View(model);
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