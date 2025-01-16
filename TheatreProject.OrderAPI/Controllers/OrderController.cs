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
    
    
}