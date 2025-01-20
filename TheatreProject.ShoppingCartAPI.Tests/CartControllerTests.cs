using Microsoft.Extensions.Logging;
using Moq;
using TheatreProject.MessageBus;
using TheatreProject.ShoppingCartAPI.Controllers;
using TheatreProject.ShoppingCartAPI.Models.DTOs;
using TheatreProject.ShoppingCartAPI.Models.Enums;
using TheatreProject.ShoppingCartAPI.Repositories.Interfaces;
using TheatreProject.ShoppingCartAPI.Services.Interfaces;

namespace TheatreProject.ShoppingCartAPI.Tests;

public class CartControllerTests
{
    private readonly Mock<ICartRepository> _cartRepositoryMock;
    private readonly Mock<ILogger<CartController>> _loggerMock;
    private readonly Mock<IMessageBus> _messageBusMock;
    private readonly Mock<ICouponService> _couponServiceMock;
    private readonly CartController _controller;

    public CartControllerTests()
    {
        _cartRepositoryMock = new Mock<ICartRepository>();
        _loggerMock = new Mock<ILogger<CartController>>();
        _messageBusMock = new Mock<IMessageBus>();
        _couponServiceMock = new Mock<ICouponService>();

        _controller = new CartController(
            _cartRepositoryMock.Object,
            _loggerMock.Object,
            _messageBusMock.Object,
            _couponServiceMock.Object);
    }

    [Fact]
    public async Task GetCart_ReturnsCartDto_WhenCartExists()
    {
        // Arrange
        var userId = "testUser";
        var expectedCart = new CartDto
        {
            CartHeader = new CartHeaderDto { UserId = userId },
            CartDetails = new List<CartDetailsDto>()
        };

        _cartRepositoryMock.Setup(x => x.GetCartByUserId(userId))
            .ReturnsAsync(expectedCart);

        // Act
        var response = await _controller.GetCart(userId);

        // Assert
        Assert.True(response.IsSuccess);
        Assert.Equal(expectedCart, response.Result);
        _cartRepositoryMock.Verify(x => x.GetCartByUserId(userId), Times.Once);
    }

    [Fact]
    public async Task GetCart_ReturnsError_WhenExceptionOccurs()
    {
        // Arrange
        var userId = "testUser";
        _cartRepositoryMock.Setup(x => x.GetCartByUserId(userId))
            .ThrowsAsync(new Exception("Test error"));

        // Act
        var response = await _controller.GetCart(userId);

        // Assert
        Assert.False(response.IsSuccess);
        Assert.NotEmpty(response.ErrorMessages);
        _cartRepositoryMock.Verify(x => x.GetCartByUserId(userId), Times.Once);
    }

    [Fact]
    public async Task GetCartDetail_ReturnsCartDetail_WhenExists()
    {
        // Arrange
        var cartDetailId = Guid.NewGuid();
        var expectedDetail = new CartDetailsDto { Id = cartDetailId };

        _cartRepositoryMock.Setup(x => x.GetCartDetail(cartDetailId))
            .ReturnsAsync(expectedDetail);

        // Act
        var response = await _controller.GetCartDetail(cartDetailId);

        // Assert
        Assert.True(response.IsSuccess);
        Assert.Equal(expectedDetail, response.Result);
        _cartRepositoryMock.Verify(x => x.GetCartDetail(cartDetailId), Times.Once);
    }
    
    [Fact]
    public async Task ValidateCart_ReturnsValidationResult()
    {
        // Arrange
        var userId = "testUser";
        var cart = new CartDto
        {
            CartHeader = new CartHeaderDto { UserId = userId }
        };
        var expectedResult = true;

        _cartRepositoryMock.Setup(x => x.GetCartByUserId(userId))
            .ReturnsAsync(cart);
        _cartRepositoryMock.Setup(x => x.ValidateCart(cart))
            .ReturnsAsync(expectedResult);

        // Act
        var response = await _controller.ValidateCart(userId);

        // Assert
        Assert.True(response.IsSuccess);
        Assert.Equal(expectedResult, response.Result);
        _cartRepositoryMock.Verify(x => x.GetCartByUserId(userId), Times.Once);
        _cartRepositoryMock.Verify(x => x.ValidateCart(cart), Times.Once);
    }

    [Fact]
    public async Task ValidateSeats_ReturnsValidationResult()
    {
        // Arrange
        var performanceId = Guid.NewGuid();
        var seats = "A1,A2";
        var expectedResult = true;

        _cartRepositoryMock.Setup(x => x.ValidateSeats(performanceId, seats))
            .ReturnsAsync(expectedResult);

        // Act
        var response = await _controller.ValidateSeats(performanceId, seats);

        // Assert
        Assert.True(response.IsSuccess);
        Assert.Equal(expectedResult, response.Result);
        _cartRepositoryMock.Verify(x => x.ValidateSeats(performanceId, seats), Times.Once);
    }

    [Fact]
    public async Task CalculateTotal_ReturnsTotal()
    {
        // Arrange
        var userId = "testUser";
        var expectedTotal = 100.0m;

        _cartRepositoryMock.Setup(x => x.CalculateTotal(userId))
            .ReturnsAsync(expectedTotal);

        // Act
        var response = await _controller.CalculateTotal(userId);

        // Assert
        Assert.True(response.IsSuccess);
        Assert.Equal(expectedTotal, response.Result);
        _cartRepositoryMock.Verify(x => x.CalculateTotal(userId), Times.Once);
    }
}