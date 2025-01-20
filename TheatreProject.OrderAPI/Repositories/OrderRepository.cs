using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using TheatreProject.OrderAPI.Data;
using TheatreProject.OrderAPI.Models;
using TheatreProject.OrderAPI.Models.DTOs;
using TheatreProject.OrderAPI.Models.Enums;
using TheatreProject.OrderAPI.Repositories.Interfaces;
using TheatreProject.OrderAPI.Services.Interfaces;

namespace TheatreProject.OrderAPI.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;
    private readonly ILogger<OrderRepository> _logger;
    private readonly IPerformanceService _performanceService;

    public OrderRepository(ApplicationDbContext db, IMapper mapper, ILogger<OrderRepository> logger,
        IPerformanceService performanceService)
    {
        _db = db;
        _mapper = mapper;
        _logger = logger;
        _performanceService = performanceService;
    }

    public async Task<IEnumerable<OrderHeader>> GetOrders(string userId)
    {
        try
        {
            IQueryable<OrderHeader> query = _db.OrderHeaders;

            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(u => u.UserId == userId);
            }

            return await query
                .Include(u => u.OrderDetails)
                .OrderByDescending(o => o.OrderTime)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting orders for user {UserId}", userId);
            throw;
        }
    }

    public async Task<OrderHeader> GetOrder(Guid orderId)
    {
        return await _db.OrderHeaders
            .Include(u => u.OrderDetails)
            .FirstOrDefaultAsync(u => u.Id == orderId);
    }

    public async Task<OrderHeader> CreateOrder(CartDto cartDto)
    {
        try
        {
            var orderHeaderDto = _mapper.Map<OrderHeaderDto>(cartDto.CartHeader);
            orderHeaderDto.Id = Guid.NewGuid();
            orderHeaderDto.OrderTime = DateTime.Now;
            orderHeaderDto.CartTotalPerformances = cartDto.CartDetails?.Count() ?? 0;
            orderHeaderDto.PaymentStatus = false;
            orderHeaderDto.Status = OrderStatus.Pending;
            orderHeaderDto.FirstName = cartDto.CartHeader.Name;
            orderHeaderDto.Email = cartDto.CartHeader.Email;
            orderHeaderDto.Phone = cartDto.CartHeader.Phone;
            orderHeaderDto.GrandTotal = Math.Round(cartDto.CartDetails?.Sum(d => d.SubTotal) ?? 0, 2);

            var orderDetails = cartDto.CartDetails.Select(cd =>
            {
                var detail = _mapper.Map<OrderDetails>(cd);
                detail.Id = Guid.NewGuid();
                detail.OrderHeaderId = orderHeaderDto.Id;
                detail.PerformanceName = cd.Performance?.Name ?? cd.Performance.Name ?? "Theatre Performance";
                return detail;
            });

            var orderHeader = _mapper.Map<OrderHeader>(orderHeaderDto);
            orderHeader.OrderDetails = orderDetails.ToList();

            _db.OrderHeaders.Add(orderHeader);
            await _db.SaveChangesAsync();

            return orderHeader;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating order from cart");
            throw;
        }
    }

    public async Task<StripeRequestDto> CreatePaymentSession(StripeRequestDto stripeRequestDto)
    {
        try
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = stripeRequestDto.ApprovedUrl,
                CancelUrl = stripeRequestDto.CancelUrl
            };

            var orderTotal = stripeRequestDto.OrderHeader.OrderDetails.Sum(item => item.SubTotal);

            var discountPercentage = !string.IsNullOrEmpty(stripeRequestDto.OrderHeader.CouponCode) ? 0.5 : 0;
            var actualDiscountAmount = orderTotal * (decimal)discountPercentage;

            stripeRequestDto.OrderHeader.DiscountTotal = actualDiscountAmount;

            foreach (var item in stripeRequestDto.OrderHeader.OrderDetails)
            {
                var productName = !string.IsNullOrEmpty(item.PerformanceName)
                    ? item.PerformanceName
                    : "Theatre Ticket";

                var originalPrice = item.PricePerTicket;
                var discountedPrice = originalPrice * (decimal)(1 - discountPercentage);
                var unitAmount = (long)(discountedPrice * 100);

                _logger.LogInformation(
                    "Item: {Name}, Original: ${Original:F2}, Discount: {DiscountPct}%, Final: ${Discounted:F2}",
                    productName, originalPrice, discountPercentage * 100, discountedPrice);

                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = unitAmount,
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = productName,
                            Description = $"Seat(s): {item.SeatNumbers}"
                        }
                    },
                    Quantity = item.Quantity
                };
                options.LineItems.Add(sessionLineItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);

            var orderHeader = await GetOrder(stripeRequestDto.OrderHeader.Id);
            if (orderHeader != null)
            {
                orderHeader.StripeSessionId = session.Id;
                orderHeader.DiscountTotal = actualDiscountAmount;
                await _db.SaveChangesAsync();
            }

            stripeRequestDto.StripeSessionId = session.Id;
            stripeRequestDto.StripeSessionUrl = session.Url;
            return stripeRequestDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment session");
            throw;
        }
    }

    public async Task<OrderHeader> ValidatePaymentSession(Guid orderHeaderId)
    {
        var orderHeader = await _db.OrderHeaders
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.Id == orderHeaderId);

        if (orderHeader == null) return null;

        var service = new SessionService();
        Session session = service.Get(orderHeader.StripeSessionId);

        var paymentIntentService = new PaymentIntentService();
        PaymentIntent paymentIntent = paymentIntentService.Get(session.PaymentIntentId);

        if (paymentIntent.Status == "succeeded")
        {
            orderHeader.PaymentIntentId = paymentIntent.Id;
            orderHeader.Status = OrderStatus.Paid;
            orderHeader.PaymentStatus = true;

            foreach (var detail in orderHeader.OrderDetails)
            {
                await _performanceService.UpdatePerformanceSeats(
                    detail.PerformanceId,
                    detail.Quantity
                );
            }

            await _db.SaveChangesAsync();
        }

        return orderHeader;
    }

    public async Task<bool> UpdateOrderStatus(Guid orderId, string newStatus)
    {
        try
        {
            var orderHeader = await _db.OrderHeaders
                .Include(x => x.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (orderHeader == null) return false;

            if (Enum.TryParse<OrderStatus>(newStatus, out OrderStatus status))
            {
                orderHeader.Status = status;

                if (status == OrderStatus.Cancelled)
                {
                    if (!string.IsNullOrEmpty(orderHeader.PaymentIntentId))
                    {
                        var options = new RefundCreateOptions
                        {
                            PaymentIntent = orderHeader.PaymentIntentId,
                            Reason = RefundReasons.RequestedByCustomer
                        };

                        var service = new RefundService();
                        var refund = await service.CreateAsync(options);

                        if (refund.Status == "succeeded")
                        {
                            orderHeader.PaymentStatus = false;
                            orderHeader.Status = OrderStatus.Refunded;
                        }
                    }
                }

                await _db.SaveChangesAsync();
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating order status");
            return false;
        }
    }
}