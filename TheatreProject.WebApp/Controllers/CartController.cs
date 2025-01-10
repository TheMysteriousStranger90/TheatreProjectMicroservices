using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using TheatreProject.WebApp.Models.DTOs;
using TheatreProject.WebApp.Models.Enums;
using TheatreProject.WebApp.Services.Interfaces;

namespace TheatreProject.WebApp.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly IPerformanceService _performanceService;
    private readonly ILogger<CartController> _logger;

    public CartController(
        ICartService cartService,
        IPerformanceService performanceService,
        ILogger<CartController> logger)
    {
        _cartService = cartService;
        _performanceService = performanceService;
        _logger = logger;
    }

    [Authorize]
    public async Task<IActionResult> Details(Guid performanceId)
    {
        try
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _performanceService.GetPerformanceByIdAsync<ResponseDto>(performanceId, token);

            if (response != null && response.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<PerformanceDto>(Convert.ToString(response.Result));
                return View(model);
            }

            TempData["error"] = response?.DisplayMessage ?? "Error fetching performance details";
            return RedirectToAction("Search", "Performance");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting performance details for ID: {Id}", performanceId);
            TempData["error"] = "Error retrieving performance details";
            return RedirectToAction("Search", "Performance");
        }
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToCart(CartDetailsDto cartDetails)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var userEmail = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                TempData["error"] = "User email not found";
                return RedirectToAction("DetailsPerformance", "Performance", new { id = cartDetails.PerformanceId });
            }

            var performanceResponse =
                await _performanceService.GetPerformanceByIdAsync<ResponseDto>(cartDetails.PerformanceId, "");
            if (!performanceResponse.IsSuccess)
            {
                TempData["error"] = "Performance not found";
                return RedirectToAction("DetailsPerformance", "Performance", new { id = cartDetails.PerformanceId });
            }

            var performance =
                JsonConvert.DeserializeObject<PerformanceDto>(Convert.ToString(performanceResponse.Result));
            decimal pricePerTicket = performance.BasePrice;

            switch (cartDetails.TicketType)
            {
                case TicketType.Student:
                    pricePerTicket *= 0.8M;
                    break;
                case TicketType.Senior:
                    pricePerTicket *= 0.85M;
                    break;
                case TicketType.VIP:
                    pricePerTicket *= 1.5M;
                    break;
            }

            CartDto cartDto = new()
            {
                CartHeader = new CartHeaderDto
                {
                    UserId = userId,
                    Email = userEmail,
                    GrandTotal = (double)(pricePerTicket * cartDetails.Quantity)
                },
                CartDetails = new List<CartDetailsDto>
                {
                    new CartDetailsDto
                    {
                        PerformanceId = cartDetails.PerformanceId,
                        Performance = performance,
                        SeatNumbers = cartDetails.SeatNumbers,
                        TicketType = cartDetails.TicketType,
                        Quantity = cartDetails.Quantity,
                        PricePerTicket = pricePerTicket,
                        SubTotal = pricePerTicket * cartDetails.Quantity
                    }
                }
            };

            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.AddToCartAsync<ResponseDto>(cartDto, token);

            if (response?.IsSuccess == true)
            {
                TempData["success"] = "Added to cart successfully";
                return RedirectToAction("Search", "Performance", new { id = cartDetails.PerformanceId });
            }

            TempData["error"] = response?.ErrorMessages?.FirstOrDefault() ?? "Failed to add to cart";
            return RedirectToAction("DetailsPerformance", "Performance", new { id = cartDetails.PerformanceId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding to cart for performance {PerformanceId}", cartDetails.PerformanceId);
            TempData["error"] = "An error occurred while adding to cart";
            return RedirectToAction("DetailsPerformance", "Performance", new { id = cartDetails.PerformanceId });
        }
    }
}