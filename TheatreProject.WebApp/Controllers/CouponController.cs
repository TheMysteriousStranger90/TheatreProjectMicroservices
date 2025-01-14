using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TheatreProject.WebApp.Models.DTOs;
using TheatreProject.WebApp.Services.Interfaces;

namespace TheatreProject.WebApp.Controllers;

[Authorize(Roles = "Administrator")]
public class CouponController : Controller
{
    private readonly ICouponService _couponService;
    private readonly ILogger<CouponController> _logger;

    public CouponController(
        ICouponService couponService,
        ILogger<CouponController> logger)
    {
        _couponService = couponService;
        _logger = logger;
    }
    
    public async Task<IActionResult> Index()
    {
        try
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _couponService.GetAllCoupons<ResponseDto>(accessToken);

            if (response != null && response.IsSuccess)
            {
                var coupons = JsonConvert.DeserializeObject<List<CouponDto>>(
                    Convert.ToString(response.Result));
                return View(coupons);
            }

            return View(new List<CouponDto>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting coupons");
            return View(new List<CouponDto>());
        }
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _couponService.DeleteCoupon<ResponseDto>(id, accessToken);

            if (response != null && response.IsSuccess)
            {
                TempData["Success"] = "Coupon deleted successfully";
            }
            else
            {
                TempData["Error"] = response?.DisplayMessage ?? "Error deleting coupon";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting coupon {Id}", id);
            TempData["Error"] = "Error deleting coupon";
        }

        return RedirectToAction(nameof(Index));
    }
}