using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheatreProject.EmailAPI.Data;
using TheatreProject.EmailAPI.Models;
using TheatreProject.EmailAPI.Models.DTOs;
using TheatreProject.EmailAPI.Services.Interfaces;

namespace TheatreProject.EmailAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmailController : ControllerBase
{
    private readonly IEmailService _emailService;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<EmailController> _logger;

    public EmailController(IEmailService emailService, ApplicationDbContext context,
        ILogger<EmailController> logger)
    {
        _emailService = emailService;
        _context = context;
        _logger = logger;
    }

    [HttpPost("SendOrderConfirmation")]
    public async Task<IActionResult> SendOrderConfirmation([FromBody] OrderConfirmationDto orderDetails)
    {
        try
        {
            await _emailService.SendOrderConfirmationAsync(orderDetails);

            // Log email
            var emailLog = new EmailLogger
            {
                Id = Guid.NewGuid(),
                Email = orderDetails.CustomerEmail,
                Message = $"Order confirmation sent for Order #{orderDetails.OrderId}",
                EmailSent = DateTime.UtcNow
            };

            await _context.EmailLoggers.AddAsync(emailLog);
            await _context.SaveChangesAsync();

            return Ok(new { Success = true, Message = "Email sent successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending order confirmation email");
            return StatusCode(500, new { Success = false, Message = ex.Message });
        }
    }

    [HttpGet("logs")]
    public async Task<IActionResult> GetEmailLogs()
    {
        var logs = await _context.EmailLoggers
            .OrderByDescending(x => x.EmailSent)
            .ToListAsync();
        return Ok(logs);
    }
}