using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TheatreProject.EmailAPI.Controllers;
using TheatreProject.EmailAPI.Data;
using TheatreProject.EmailAPI.Models;
using TheatreProject.EmailAPI.Models.DTOs;
using TheatreProject.EmailAPI.Services.Interfaces;

namespace TheatreProject.EmailAPI.Tests;

public class EmailControllerTests
{
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly Mock<ILogger<EmailController>> _loggerMock;
    private readonly ApplicationDbContext _context;
    private readonly EmailController _controller;

    public EmailControllerTests()
    {
        _emailServiceMock = new Mock<IEmailService>();
        _loggerMock = new Mock<ILogger<EmailController>>();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        _context = new ApplicationDbContext(options);
        _controller = new EmailController(_emailServiceMock.Object, _context, _loggerMock.Object);
    }

    [Fact]
    public async Task SendOrderConfirmation_ValidRequest_ReturnsOk()
    {
        // Arrange
        var orderConfirmation = new OrderConfirmationDto
        {
            OrderId = Guid.NewGuid(),
            CustomerEmail = "test@example.com",
            CustomerName = "Test User"
        };

        _emailServiceMock.Setup(x => x.SendOrderConfirmationAsync(It.IsAny<OrderConfirmationDto>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.SendOrderConfirmation(orderConfirmation);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var properties = okResult.Value.GetType().GetProperties();
        
        var successProperty = properties.FirstOrDefault(p => p.Name == "Success");
        var messageProperty = properties.FirstOrDefault(p => p.Name == "Message");
        
        Assert.NotNull(successProperty);
        Assert.NotNull(messageProperty);
        Assert.True((bool)successProperty.GetValue(okResult.Value));
        Assert.Equal("Email sent successfully", (string)messageProperty.GetValue(okResult.Value));
        
        var log = await _context.EmailLoggers.FirstOrDefaultAsync();
        Assert.NotNull(log);
        Assert.Equal(orderConfirmation.CustomerEmail, log.Email);
    }

    [Fact]
    public async Task SendOrderConfirmation_EmailServiceThrows_Returns500()
    {
        // Arrange
        var orderConfirmation = CreateTestOrderConfirmation();
        _emailServiceMock.Setup(x => x.SendOrderConfirmationAsync(It.IsAny<OrderConfirmationDto>()))
            .ThrowsAsync(new Exception("Test error"));

        // Act
        var result = await _controller.SendOrderConfirmation(orderConfirmation);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        
        var properties = statusCodeResult.Value.GetType().GetProperties();
        var successProperty = properties.FirstOrDefault(p => p.Name == "Success");
        var messageProperty = properties.FirstOrDefault(p => p.Name == "Message");
        
        Assert.NotNull(successProperty);
        Assert.NotNull(messageProperty);
        Assert.False((bool)successProperty.GetValue(statusCodeResult.Value));
        Assert.Equal("Test error", (string)messageProperty.GetValue(statusCodeResult.Value));
    }

    private static OrderConfirmationDto CreateTestOrderConfirmation()
    {
        return new OrderConfirmationDto
        {
            OrderId = Guid.NewGuid(),
            CustomerEmail = "test@example.com",
            CustomerName = "Test User",
            OrderDate = DateTime.UtcNow,
            OrderDetails = new List<OrderDetailsDto>(),
            TotalAmount = 100.00
        };
    }

    [Fact]
    public async Task GetEmailLogs_ReturnsAllLogs()
    {
        // Arrange
        var logs = new List<EmailLogger>
        {
            new() 
            { 
                Id = Guid.NewGuid(), 
                Email = "test1@example.com",
                Message = "Test message 1", 
                EmailSent = DateTime.UtcNow 
            },
            new() 
            { 
                Id = Guid.NewGuid(), 
                Email = "test2@example.com",
                Message = "Test message 2", 
                EmailSent = DateTime.UtcNow.AddMinutes(-5) 
            }
        };

        await _context.EmailLoggers.AddRangeAsync(logs);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetEmailLogs();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedLogs = Assert.IsType<List<EmailLogger>>(okResult.Value);
        Assert.Equal(2, returnedLogs.Count);
        Assert.Equal(logs[0].Email, returnedLogs[0].Email);
        Assert.Equal(logs[0].Message, returnedLogs[0].Message);
    }
}