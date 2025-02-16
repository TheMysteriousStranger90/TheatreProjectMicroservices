﻿using Microsoft.AspNetCore.Authorization;
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
    private readonly ICouponService _couponService;
    private readonly IPerformanceService _performanceService;
    private readonly ILogger<CartController> _logger;
    private readonly IOrderService _orderService;

    public CartController(
        ICartService cartService,
        IPerformanceService performanceService,
        ICouponService couponService,
        IOrderService orderService,
        ILogger<CartController> logger)
    {
        _cartService = cartService;
        _performanceService = performanceService;
        _couponService = couponService;
        _orderService = orderService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        return View(await LoadCartByUser());
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Checkout(CartDto cartDto)
    {
        try
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var currentCart = await LoadCartByUser();
            currentCart.CartHeader.Phone = cartDto.CartHeader.Phone;
            currentCart.CartHeader.Email = cartDto.CartHeader.Email;
            currentCart.CartHeader.FirstName = cartDto.CartHeader.FirstName;
            currentCart.CartHeader.LastName = cartDto.CartHeader.LastName;

            var orderResponse = await _orderService.CreateOrderAsync<ResponseDto>(currentCart, accessToken);
            if (!orderResponse.IsSuccess)
            {
                TempData["Error"] = orderResponse.DisplayMessage;
                return RedirectToAction(nameof(Checkout));
            }

            var orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(
                Convert.ToString(orderResponse.Result));

            var domain = $"{Request.Scheme}://{Request.Host.Value}/";
            var stripeRequestDto = new StripeRequestDto
            {
                ApprovedUrl = domain + $"cart/Confirmation?orderId={orderHeaderDto.Id}",
                CancelUrl = domain + "cart/Checkout",
                OrderHeader = orderHeaderDto,
                CustomerEmail = cartDto.CartHeader.Email,
                CustomerName = $"{cartDto.CartHeader.FirstName} {cartDto.CartHeader.LastName}",
                CustomerPhone = cartDto.CartHeader.Phone,
                PaymentMethodData = new()
                {
                    BillingDetails = new()
                    {
                        Email = cartDto.CartHeader.Email,
                        Name = $"{cartDto.CartHeader.FirstName} {cartDto.CartHeader.LastName}",
                        Phone = cartDto.CartHeader.Phone
                    }
                }
            };

            var stripeResponse = await _orderService.CreatePaymentSessionAsync<ResponseDto>(
                stripeRequestDto, accessToken);

            if (!stripeResponse.IsSuccess)
            {
                TempData["Error"] = "Error creating payment session";
                return RedirectToAction(nameof(Checkout));
            }

            var stripeSessionDto = JsonConvert.DeserializeObject<StripeRequestDto>(
                Convert.ToString(stripeResponse.Result));

            Response.Headers.Add("Location", stripeSessionDto.StripeSessionUrl);
            return new StatusCodeResult(303);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during checkout");
            TempData["Error"] = "An error occurred during checkout";
            return RedirectToAction(nameof(Checkout));
        }
    }

    public async Task<IActionResult> Index()
    {
        return View(await LoadCartByUser());
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Confirmation(Guid? orderId = null)
    {
        if (!orderId.HasValue)
        {
            return View();
        }

        try
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _orderService.ValidatePaymentAsync<ResponseDto>(orderId.Value, accessToken);

            if (response?.IsSuccess == true)
            {
                var orderHeader = JsonConvert.DeserializeObject<OrderHeaderDto>(
                    Convert.ToString(response.Result));

                if (orderHeader.PaymentStatus)
                {
                    await _cartService.ClearCartAsync<ResponseDto>(orderHeader.UserId, accessToken);
                    return View(orderId);
                }
            }

            return RedirectToAction("Failed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing order confirmation");
            return RedirectToAction("Failed");
        }
    }

    public IActionResult Failed()
    {
        return View();
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
    public async Task<IActionResult> AddToCart(Guid performanceId)
    {
        try
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _performanceService.GetPerformanceByIdAsync<ResponseDto>(performanceId, token);

            if (response?.IsSuccess != true)
            {
                TempData["error"] = "Performance not found";
                return RedirectToAction("Search", "Performance");
            }

            var performance = JsonConvert.DeserializeObject<PerformanceDto>(Convert.ToString(response.Result));
            return View(performance);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error preparing add to cart view");
            TempData["error"] = "Error loading booking form";
            return RedirectToAction("Search", "Performance");
        }
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToCart(CartDetailsDto cartDetails)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var userEmail = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email)?.Value;
            var firstName = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.GivenName)?.Value;
            var lastName = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.FamilyName)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                TempData["error"] = "User email not found";
                return RedirectToAction("AddToCart", new { performanceId = cartDetails.PerformanceId });
            }

            var performanceResponse =
                await _performanceService.GetPerformanceByIdAsync<ResponseDto>(cartDetails.PerformanceId, "");
            if (!performanceResponse.IsSuccess)
            {
                TempData["error"] = "Performance not found";
                return RedirectToAction("AddToCart", new { performanceId = cartDetails.PerformanceId });
            }

            var performance =
                JsonConvert.DeserializeObject<PerformanceDto>(Convert.ToString(performanceResponse.Result));
            decimal pricePerTicket = CalculateTicketPrice(performance.BasePrice, cartDetails.TicketType);
            var cartDto = new CartDto
            {
                CartHeader = new CartHeaderDto
                {
                    UserId = userId,
                    Email = userEmail,
                    FirstName = firstName,
                    LastName = lastName,
                    GrandTotal = (pricePerTicket * cartDetails.Quantity)
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
                return RedirectToAction("Index", "Cart");
            }

            TempData["error"] = response?.ErrorMessages?.FirstOrDefault() ?? "Failed to add to cart";
            return RedirectToAction("AddToCart", new { performanceId = cartDetails.PerformanceId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding to cart");
            TempData["error"] = "An error occurred while adding to cart";
            return RedirectToAction("AddToCart", new { performanceId = cartDetails.PerformanceId });
        }
    }

    private decimal CalculateTicketPrice(decimal basePrice, TicketType ticketType)
    {
        return ticketType switch
        {
            TicketType.Student => basePrice * 0.8M,
            TicketType.Senior => basePrice * 0.85M,
            TicketType.VIP => basePrice * 1.5M,
            _ => basePrice
        };
    }

    private async Task<CartDto> LoadCartByUser()
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var userEmail = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email)?.Value;
            var firstName = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.GivenName)?.Value;
            var lastName = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.FamilyName)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return new CartDto
                {
                    CartHeader = new CartHeaderDto(),
                    CartDetails = new List<CartDetailsDto>()
                };
            }

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.GetCartByUserIdAsync<ResponseDto>(userId, accessToken);

            if (response?.IsSuccess == true && response.Result != null)
            {
                var cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));

                if (cartDto?.CartHeader != null && cartDto.CartDetails != null)
                {
                    cartDto.CartHeader.Email = userEmail;
                    cartDto.CartHeader.FirstName = firstName;
                    cartDto.CartHeader.LastName = lastName;
                    
                    decimal originalTotal = 0;
                    foreach (var detail in cartDto.CartDetails)
                    {
                        if (detail.Performance == null)
                        {
                            var performanceResponse = await _performanceService
                                .GetPerformanceByIdAsync<ResponseDto>(detail.PerformanceId, accessToken);

                            if (performanceResponse?.IsSuccess == true)
                            {
                                detail.Performance = JsonConvert.DeserializeObject<PerformanceDto>(
                                    Convert.ToString(performanceResponse.Result));
                            }
                        }

                        originalTotal += detail.PricePerTicket * detail.Quantity;
                    }

                    decimal discountAmount = 0;
                    if (!string.IsNullOrEmpty(cartDto.CartHeader.CouponCode))
                    {
                        var coupon = await _couponService.GetCoupon<ResponseDto>(
                            cartDto.CartHeader.CouponCode, accessToken);

                        if (coupon != null && coupon.IsSuccess)
                        {
                            var couponObj = JsonConvert.DeserializeObject<CouponDto>(
                                Convert.ToString(coupon.Result));

                            discountAmount = originalTotal * ((decimal)couponObj.DiscountAmount / 100);
                            cartDto.CartHeader.DiscountTotal = couponObj.DiscountAmount;
                        }
                    }

                    cartDto.CartHeader.GrandTotal = (originalTotal - discountAmount);
                }

                return cartDto;
            }


            return new CartDto
            {
                CartHeader = new CartHeaderDto
                {
                    Email = userEmail,
                    FirstName = firstName,
                    LastName = lastName
                },
                CartDetails = new List<CartDetailsDto>()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading cart");
            return new CartDto
            {
                CartHeader = new CartHeaderDto(),
                CartDetails = new List<CartDetailsDto>()
            };
        }
    }

    public async Task<IActionResult> RemoveFromCart(Guid cartDetailsId)
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var response = await _cartService.RemoveFromCartAsync<ResponseDto>(cartDetailsId, accessToken);

        if (response.IsSuccess)
        {
            return RedirectToAction(nameof(Index));
        }

        return View();
    }

    public async Task<IActionResult> ClearCart()
    {
        var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var response = await _cartService.ClearCartAsync<ResponseDto>(userId, accessToken);

        if (response.IsSuccess)
        {
            return RedirectToAction(nameof(Index));
        }

        return View();
    }


    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UpdateQuantity(Guid cartDetailId, int quantity)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var response = await _cartService.UpdateQuantityAsync<ResponseDto>(cartDetailId, quantity, token);

        if (response.IsSuccess)
        {
            TempData["success"] = "Quantity updated successfully";
            return RedirectToAction(nameof(Index));
        }

        TempData["error"] = "Failed to update quantity";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ValidateSeats(Guid performanceId, string seats)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var response = await _cartService.ValidateSeatsAsync<ResponseDto>(performanceId, seats, token);

        return Json(new { isValid = response.IsSuccess });
    }

    [Authorize]
    public async Task<IActionResult> GetCartStatus()
    {
        var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
        var token = await HttpContext.GetTokenAsync("access_token");
        var response = await _cartService.GetCartStatusAsync<ResponseDto>(userId, token);

        return Json(response.Result);
    }

    [Authorize]
    public async Task<IActionResult> CalculateTotal()
    {
        var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
        var token = await HttpContext.GetTokenAsync("access_token");
        var response = await _cartService.CalculateTotalAsync<ResponseDto>(userId, token);

        return Json(new { total = response.Result });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SaveForLater()
    {
        var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
        var token = await HttpContext.GetTokenAsync("access_token");
        var response = await _cartService.SaveCartForLaterAsync<ResponseDto>(userId, token);

        if (response.IsSuccess)
        {
            TempData["success"] = "Cart saved for later";
            return RedirectToAction(nameof(Index));
        }

        TempData["error"] = "Failed to save cart";
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    public async Task<IActionResult> GetCartDetail(Guid cartDetailId)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var response = await _cartService.GetCartDetailAsync<ResponseDto>(cartDetailId, token);

        if (response.IsSuccess)
        {
            var detail = JsonConvert.DeserializeObject<CartDetailsDto>(Convert.ToString(response.Result));
            return PartialView("_CartDetailPartial", detail);
        }

        return NotFound();
    }

    [HttpPost]
    [ActionName("ApplyCoupon")]
    public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
    {
        try
        {
            var currentCart = await LoadCartByUser();
            currentCart.CartHeader.CouponCode = cartDto.CartHeader.CouponCode;

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.ApplyCoupon<ResponseDto>(currentCart, accessToken);

            if (response.IsSuccess)
            {
                TempData["success"] = "Coupon applied successfully";
            }
            else
            {
                TempData["error"] = response.DisplayMessage ?? "Failed to apply coupon";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error applying coupon");
            TempData["error"] = "An error occurred while applying the coupon";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ActionName("RemoveCoupon")]
    public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var response = await _cartService.RemoveCoupon<ResponseDto>(cartDto.CartHeader.UserId, accessToken);

        if (response != null && response.IsSuccess)
        {
            return RedirectToAction(nameof(Index));
        }

        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ValidateCart()
    {
        var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
        var token = await HttpContext.GetTokenAsync("access_token");
        var response = await _cartService.ValidateCartAsync<ResponseDto>(userId, token);

        return Json(new { isValid = response.IsSuccess });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ValidateCouponForCart(CartDto cartDto)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var response = await _cartService.ValidateCouponForCartAsync<ResponseDto>(cartDto, token);

        return Json(response.Result);
    }
}