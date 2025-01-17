using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;
using TheatreProject.WebApp.Models.DTOs;
using TheatreProject.WebApp.Services.Interfaces;

namespace TheatreProject.WebApp.Controllers;

public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrderController> _logger;
    protected ResponseDto _response;

    public OrderController(IOrderService orderService, ILogger<OrderController> logger)
    {
        _orderService = orderService;
        _logger = logger;
        _response = new ResponseDto();
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _orderService.GetOrdersAsync<ResponseDto>(userId, token);

            if (response != null && response.IsSuccess)
            {
                var orders = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(
                    Convert.ToString(response.Result));
                return View(orders);
            }

            TempData["error"] = response?.DisplayMessage ?? "Error fetching orders";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting orders");
            TempData["error"] = "Error retrieving orders";
        }
        return View(new List<OrderHeaderDto>());
    }

    [Authorize]
    public async Task<IActionResult> Details(Guid orderId)
    {
        try
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _orderService.GetOrderAsync<ResponseDto>(orderId, token);

            if (response != null && response.IsSuccess)
            {
                var order = JsonConvert.DeserializeObject<OrderHeaderDto>(
                    Convert.ToString(response.Result));
                return View(order);
            }

            TempData["error"] = response?.DisplayMessage ?? "Error fetching order details";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting order details for ID: {Id}", orderId);
            TempData["error"] = "Error retrieving order details";
        }
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CancelOrder(Guid orderId)
    {
        try
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _orderService.UpdateOrderStatusAsync<ResponseDto>(
                orderId, "Cancelled", token);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Order cancelled successfully";
                return RedirectToAction(nameof(Details), new { orderId });
            }

            TempData["error"] = response?.DisplayMessage ?? "Error cancelling order";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling order ID: {Id}", orderId);
            TempData["error"] = "Error cancelling order";
        }
        return RedirectToAction(nameof(Details), new { orderId });
    }
}