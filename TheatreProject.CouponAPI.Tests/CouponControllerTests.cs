
using Moq;
using TheatreProject.CouponAPI.Controllers;
using TheatreProject.CouponAPI.Models.DTOs;
using TheatreProject.CouponAPI.Repositories.Interfaces;

namespace TheatreProject.CouponAPI.Tests;

public class CouponControllerTests
{
    private readonly Mock<ICouponRepository> _mockRepo;
    private readonly CouponController _controller;

    public CouponControllerTests()
    {
        _mockRepo = new Mock<ICouponRepository>();
        _controller = new CouponController(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsAllCoupons_WhenCouponsExist()
    {
        // Arrange
        var expectedCoupons = new List<CouponDto>
        {
            new() { Id = Guid.NewGuid(), CouponCode = "TEST10", DiscountAmount = 10 },
            new() { Id = Guid.NewGuid(), CouponCode = "TEST20", DiscountAmount = 20 }
        };
        _mockRepo.Setup(repo => repo.GetAllCoupons())
            .ReturnsAsync(expectedCoupons);

        // Act
        var result = await _controller.GetAll();

        // Assert
        Assert.True(result.IsSuccess);
        var coupons = Assert.IsType<List<CouponDto>>(result.Result);
        Assert.Equal(2, coupons.Count);
    }

    [Fact]
    public async Task GetByCode_ReturnsCoupon_WhenCouponExists()
    {
        // Arrange
        var expectedCoupon = new CouponDto 
        { 
            Id = Guid.NewGuid(), 
            CouponCode = "TEST10", 
            DiscountAmount = 10 
        };
        _mockRepo.Setup(repo => repo.GetCouponByCode("TEST10"))
            .ReturnsAsync(expectedCoupon);

        // Act
        var result = await _controller.GetByCode("TEST10");

        // Assert
        Assert.True(result.IsSuccess);
        var coupon = Assert.IsType<CouponDto>(result.Result);
        Assert.Equal("TEST10", coupon.CouponCode);
    }

    [Fact]
    public async Task Create_ReturnsCoupon_WhenValidData()
    {
        // Arrange
        var newCoupon = new CouponDto 
        { 
            CouponCode = "NEW10", 
            DiscountAmount = 10 
        };
        var response = new ResponseDto 
        { 
            IsSuccess = true, 
            Result = newCoupon 
        };
        _mockRepo.Setup(repo => repo.CreateCoupon(It.IsAny<CouponDto>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.Create(newCoupon);

        // Assert
        Assert.True(result.IsSuccess);
        var coupon = Assert.IsType<CouponDto>(result.Result);
        Assert.Equal("NEW10", coupon.CouponCode);
    }
}