using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheatreProject.MessageBus;
using TheatreProject.OrderAPI.Models.DTOs;
using TheatreProject.OrderAPI.Repositories.Interfaces;
using TheatreProject.OrderAPI.Services.Interfaces;

namespace TheatreProject.OrderAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : Controller
{
    protected ResponseDto _response;
    private readonly ILogger<OrderController> _logger;
    private IPerformanceService _performanceService;
    private IOrderRepository _orderRepository;
    
    private readonly IMessageBus _messageBus;
    private readonly IConfiguration _configuration;
    
    public OrderController(ILogger<OrderController> logger, IPerformanceService performanceService, IOrderRepository orderRepository, IMessageBus messageBus, IConfiguration configuration)
    {
        _logger = logger;
        _performanceService = performanceService;
        _orderRepository = orderRepository;
        _messageBus = messageBus;
        _configuration = configuration;
        this._response = new ResponseDto();
    }
    
    [HttpGet("GetOrders")]
    [Authorize]
    public async Task<ActionResult<ResponseDto>> GetOrders([FromQuery] string? userId = "")
    {
        try
        {
            var orders = await _orderRepository.GetOrders(userId);
            _response.Result = orders;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
            _logger.LogError(ex, "Error getting orders");
        }
        return _response;
    }
    
    [HttpGet("GetOrder/{orderId}")]
    [Authorize]
    public async Task<ActionResult<ResponseDto>> GetOrder(Guid orderId)
    {
        try
        {
            var order = await _orderRepository.GetOrder(orderId);
            _response.Result = order;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
            _logger.LogError(ex, "Error getting order {OrderId}", orderId);
        }
        return _response;
    }
    
    [HttpPost("CreateOrder")]
    [Authorize]
    public async Task<ActionResult<ResponseDto>> CreateOrder([FromBody] CartDto cartDto)
    {
        try
        {
            var order = await _orderRepository.CreateOrder(cartDto);
            _response.Result = order;
            
            /*
            // Publish order created event
            var topicName = _configuration.GetValue<string>("MessageBus:OrderCreatedTopic");
            await _messageBus.PublishMessage(order, topicName);
            */
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
            _logger.LogError(ex, "Error creating order");
        }
        return _response;
    }
    
    [HttpPost("CreatePaymentSession")]
    [Authorize]
    public async Task<ActionResult<ResponseDto>> CreatePaymentSession([FromBody] StripeRequestDto stripeRequestDto)
    {
        try
        {
            var stripeResponse = await _orderRepository.CreatePaymentSession(stripeRequestDto);
            _response.Result = stripeResponse;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
            _logger.LogError(ex, "Error creating payment session");
        }
        return _response;
    }
    
    [HttpPost("ValidatePayment/{orderId}")]
    [Authorize]
    public async Task<ActionResult<ResponseDto>> ValidatePayment(Guid orderId)
    {
        try
        {
            var validatedOrder = await _orderRepository.ValidatePaymentSession(orderId);
            _response.Result = validatedOrder;

            if (validatedOrder?.PaymentStatus == true)
            {
                /*
                // Publish payment successful event
                var topicName = _configuration.GetValue<string>("MessageBus:PaymentSuccessfulTopic");
                await _messageBus.PublishMessage(validatedOrder, topicName);
                */
            }
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
            _logger.LogError(ex, "Error validating payment for order {OrderId}", orderId);
        }
        return _response;
    }
    
    [HttpPost("UpdateOrderStatus/{orderId}")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<ResponseDto>> UpdateOrderStatus(Guid orderId, [FromBody] string newStatus)
    {
        try
        {
            var result = await _orderRepository.UpdateOrderStatus(orderId, newStatus);
            _response.Result = result;

            if (result)
            {
                /*
                // Publish order status changed event
                var topicName = _configuration.GetValue<string>("MessageBus:OrderStatusChangedTopic");
                await _messageBus.PublishMessage(new { OrderId = orderId, NewStatus = newStatus }, topicName);
                */
            }
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
            _logger.LogError(ex, "Error updating order status for order {OrderId}", orderId);
        }
        return _response;
    }
}