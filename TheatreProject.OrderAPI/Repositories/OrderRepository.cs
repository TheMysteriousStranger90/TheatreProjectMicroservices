using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using TheatreProject.OrderAPI.Data;
using TheatreProject.OrderAPI.Models;
using TheatreProject.OrderAPI.Models.DTOs;
using TheatreProject.OrderAPI.Repositories.Interfaces;

namespace TheatreProject.OrderAPI.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;
    private readonly ILogger<OrderRepository> _logger;

    public OrderRepository(ApplicationDbContext db, IMapper mapper, ILogger<OrderRepository> logger)
    {
        _db = db;
        _mapper = mapper;
        _logger = logger;
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
            // Map cart header to order header
            var orderHeaderDto = _mapper.Map<OrderHeaderDto>(cartDto.CartHeader);
            orderHeaderDto.OrderTime = DateTime.Now;
            orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetails>>(cartDto.CartDetails);
            orderHeaderDto.CartTotalPerformances = cartDto.CartDetails?.Count() ?? 0;
            orderHeaderDto.PaymentStatus = false;

            orderHeaderDto.GrandTotal = Math.Round(cartDto.CartDetails?.Sum(d => d.SubTotal) ?? 0, 2);

            var orderHeader = _mapper.Map<OrderHeader>(orderHeaderDto);

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

    public async Task<bool> AddOrder(OrderHeader orderHeader)
    {
        try
        {
            await _db.OrderHeaders.AddAsync(orderHeader);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding order");
            return false;
        }
    }

    public async Task<StripeRequestDto> CreatePaymentSession(StripeRequestDto stripeRequestDto)
    {
        try
        {
            var options = new SessionCreateOptions
            {
                SuccessUrl = stripeRequestDto.ApprovedUrl,
                CancelUrl = stripeRequestDto.CancelUrl,
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            var discountObj = new List<SessionDiscountOptions>();
            if (!string.IsNullOrEmpty(stripeRequestDto.OrderHeader.CouponCode))
            {
                discountObj.Add(new SessionDiscountOptions
                {
                    Coupon = stripeRequestDto.OrderHeader.CouponCode
                });
                options.Discounts = discountObj;
            }

            foreach (var item in stripeRequestDto.OrderHeader.OrderDetails)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.PricePerTicket * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.PerformanceName
                        }
                    },
                    Quantity = item.Quantity
                };
                options.LineItems.Add(sessionLineItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);

            var orderHeader = await GetOrder(stripeRequestDto.OrderHeader.Id);
            orderHeader.StripeSessionId = session.Id;
            await _db.SaveChangesAsync();

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
        var orderHeader = await _db.OrderHeaders.FirstOrDefaultAsync(o => o.Id == orderHeaderId);
        if (orderHeader == null) return null;

        var service = new SessionService();
        Session session = service.Get(orderHeader.StripeSessionId);

        var paymentIntentService = new PaymentIntentService();
        PaymentIntent paymentIntent = paymentIntentService.Get(session.PaymentIntentId);

        if (paymentIntent.Status == "succeeded")
        {
            orderHeader.PaymentIntentId = paymentIntent.Id;
            orderHeader.PaymentStatus = true;
            await _db.SaveChangesAsync();
        }

        return orderHeader;
    }

    public async Task<bool> UpdateOrderStatus(Guid orderId, string newStatus)
    {
        try
        {
            var orderHeader = await _db.OrderHeaders.FindAsync(orderId);
            if (orderHeader == null) return false;

            if (newStatus == "Cancelled" && !string.IsNullOrEmpty(orderHeader.PaymentIntentId))
            {
                var options = new RefundCreateOptions
                {
                    PaymentIntent = orderHeader.PaymentIntentId,
                    Reason = RefundReasons.RequestedByCustomer
                };

                var service = new RefundService();
                Refund refund = service.Create(options);
            }

            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating order status");
            return false;
        }
    }
}