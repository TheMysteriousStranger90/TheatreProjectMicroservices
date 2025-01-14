using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using TheatreProject.PerformanceAPI.Controllers;
using TheatreProject.PerformanceAPI.Models;
using TheatreProject.PerformanceAPI.Models.DTOs;
using TheatreProject.PerformanceAPI.Repositories;
using TheatreProject.PerformanceAPI.Services;

namespace TheatreProject.PerformanceAPI.Tests
{
    public class PerformanceControllerTests
    {
        private readonly Mock<IPerformanceRepository> _mockRepo;
        private readonly Mock<ILogger<PerformanceController>> _mockLogger;
        private readonly Mock<IMemoryCache> _mockCache;
        private readonly Mock<ICacheKeyService> _mockCacheKeyService;
        private readonly PerformanceController _controller;

        public PerformanceControllerTests()
        {
            _mockRepo = new Mock<IPerformanceRepository>();
            _mockLogger = new Mock<ILogger<PerformanceController>>();
            _mockCache = new Mock<IMemoryCache>();
            _mockCacheKeyService = new Mock<ICacheKeyService>();

            _controller = new PerformanceController(
                _mockRepo.Object,
                _mockLogger.Object,
                _mockCache.Object,
                _mockCacheKeyService.Object
            );
        }

        [Fact]
        public async Task GetPerformances_ReturnsAllPerformances()
        {
            // Arrange
            var expectedPerformances = new List<PerformanceDto>
            {
                new() { Id = Guid.NewGuid(), Name = "Test Performance 1" },
                new() { Id = Guid.NewGuid(), Name = "Test Performance 2" }
            };

            _mockRepo.Setup(repo => repo.GetPerformances())
                .ReturnsAsync(expectedPerformances);

            // Act
            var result = await _controller.GetPerformances();

            // Assert
            var response = Assert.IsType<ResponseDto>(result);
            Assert.True(response.IsSuccess);
            var performances = Assert.IsAssignableFrom<IEnumerable<PerformanceDto>>(response.Result);
            Assert.Equal(2, performances.Count());
        }

        [Fact]
        public async Task GetPerformanceById_WithValidId_ReturnsPerformance()
        {
            // Arrange
            var performanceId = Guid.NewGuid();
            var expectedPerformance = new PerformanceDto { Id = performanceId, Name = "Test Performance" };

            _mockRepo.Setup(repo => repo.GetPerformanceById(performanceId))
                .ReturnsAsync(expectedPerformance);

            // Act
            var result = await _controller.GetPerformanceById(performanceId);

            // Assert
            var response = Assert.IsType<ResponseDto>(result);
            Assert.True(response.IsSuccess);
            var performance = Assert.IsType<PerformanceDto>(response.Result);
            Assert.Equal(performanceId, performance.Id);
        }

        [Fact]
        public async Task CreatePerformance_WithValidData_ReturnsCreatedPerformance()
        {
            // Arrange
            var performanceDto = new CreatePerformanceDto
            {
                Name = "New Performance",
                Description = "Test Description",
                TheatreName = "Test Theatre",
                City = "Test City",
                Address = "Test Address",
                Capacity = 100,
                BasePrice = 50.00m,
                ShowDateTime = DateTime.Now.AddDays(1),
                Duration = TimeSpan.FromHours(2),
                Category = TheatreCategory.Drama,
                Image = null
            };

            var createdPerformance = new PerformanceDto
            {
                Id = Guid.NewGuid(),
                Name = performanceDto.Name,
                Description = performanceDto.Description,
                TheatreName = performanceDto.TheatreName,
                City = performanceDto.City,
                Address = performanceDto.Address,
                Capacity = performanceDto.Capacity,
                BasePrice = performanceDto.BasePrice,
                ShowDateTime = performanceDto.ShowDateTime,
                Duration = performanceDto.Duration,
                Category = performanceDto.Category,
                AvailableSeats = performanceDto.Capacity,
                CreatedDate = DateTime.UtcNow
            };

            _mockRepo.Setup(repo => repo.CreateUpdatePerformance(It.IsAny<CreatePerformanceDto>()))
                .ReturnsAsync(createdPerformance);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7000");

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            _controller.ModelState.Clear();

            // Act
            var result = await _controller.CreatePerformance(performanceDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ResponseDto>>(result);
            var response = actionResult.Value;
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);
            var returnedPerformance = Assert.IsType<PerformanceDto>(response.Result);
            Assert.Equal(performanceDto.Name, returnedPerformance.Name);
            Assert.Equal(performanceDto.TheatreName, returnedPerformance.TheatreName);
            Assert.Equal(performanceDto.BasePrice, returnedPerformance.BasePrice);

            _mockCacheKeyService.Verify(x =>
                x.GetKeysStartingWith("Performances"), Times.Once());
        }

        [Fact]
        public async Task DeletePerformance_WithValidId_ReturnsSuccess()
        {
            // Arrange
            var performanceId = Guid.NewGuid();
            _mockRepo.Setup(repo => repo.DeletePerformance(performanceId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeletePerformance(performanceId);

            // Assert
            var response = Assert.IsType<ResponseDto>(result);
            Assert.True(response.IsSuccess);
            Assert.True((bool)response.Result);

            // Verify cache was cleared
            _mockCacheKeyService.Verify(service =>
                service.GetKeysStartingWith(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UpdatePerformanceStatus_WithValidData_UpdatesStatus()
        {
            // Arrange
            var performanceId = Guid.NewGuid();
            var status = PerformanceStatus.Cancelled;

            _mockRepo.Setup(repo => repo.UpdatePerformanceStatus(performanceId, status))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdatePerformanceStatus(performanceId, status);

            // Assert
            var response = Assert.IsType<ResponseDto>(result);
            Assert.True(response.IsSuccess);
            Assert.True((bool)response.Result);

            // Verify cache was cleared
            _mockCacheKeyService.Verify(service =>
                service.RemoveKey(It.IsAny<string>()), Times.Once);
        }
    }
}