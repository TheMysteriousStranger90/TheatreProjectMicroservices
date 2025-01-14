using System.Net;
using System.Security.Claims;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TheatreProject.Identity.MainModule.Account;
using TheatreProject.Identity.Models;
using TheatreProject.Identity.Services.Interfaces;

namespace TheatreProject.Identity.Tests;

public class AccountControllerTests
{
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<SignInManager<ApplicationUser>> _mockSignInManager;
    private readonly Mock<RoleManager<IdentityRole>> _mockRoleManager;
    private readonly Mock<ILogger<AccountController>> _mockLogger;
    private readonly Mock<IIdentityServerInteractionService> _mockInteraction;
    private readonly Mock<IClientStore> _mockClientStore;
    private readonly Mock<IAuthenticationSchemeProvider> _mockSchemeProvider;
    private readonly Mock<IEventService> _mockEvents;
    private readonly Mock<IRemoteHostService> _mockRemoteHostService;
    private readonly Mock<IIpBlockingService> _mockIpBlockingService;


    public AccountControllerTests()
    {
        var userStore = new Mock<IUserStore<ApplicationUser>>();
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(
            userStore.Object, null, null, null, null, null, null, null, null);

        _mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
            _mockUserManager.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
            null, null, null, null);

        _mockRoleManager = new Mock<RoleManager<IdentityRole>>(
            new Mock<IRoleStore<IdentityRole>>().Object,
            null, null, null, null);

        _mockLogger = new Mock<ILogger<AccountController>>();
        _mockInteraction = new Mock<IIdentityServerInteractionService>();
        _mockClientStore = new Mock<IClientStore>();
        _mockSchemeProvider = new Mock<IAuthenticationSchemeProvider>();
        _mockEvents = new Mock<IEventService>();
        _mockRemoteHostService = new Mock<IRemoteHostService>();
        _mockIpBlockingService = new Mock<IIpBlockingService>();
    }

    [Fact]
    public async Task Register_WithValidModel_CreatesUserAndAssignsRole()
    {
        // Arrange
        var testIp = IPAddress.Parse("127.0.0.1");
        var model = new RegisterViewModel
        {
            Username = "testuser",
            Email = "test@test.com",
            Password = "Test123!",
            FirstName = "Test",
            LastName = "User",
            ReturnUrl = "/"
        };

        var mockHttpContext = new Mock<HttpContext>();
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper.Setup(x => x.IsLocalUrl(It.IsAny<string>())).Returns(true);

        _mockRemoteHostService
            .Setup(x => x.GetRemoteHostIpAddressUsingRemoteIpAddress(It.IsAny<HttpContext>()))
            .Returns(testIp);

        _mockIpBlockingService
            .Setup(x => x.IsBlocked(testIp))
            .Returns(false);

        _mockUserManager
            .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), model.Password))
            .ReturnsAsync(IdentityResult.Success);

        _mockRoleManager
            .Setup(x => x.RoleExistsAsync("Customer"))
            .ReturnsAsync(true);

        _mockUserManager
            .Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Customer"))
            .ReturnsAsync(IdentityResult.Success);

        _mockUserManager
            .Setup(x => x.AddClaimsAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<Claim>>()))
            .ReturnsAsync(IdentityResult.Success);

        _mockSignInManager
            .Setup(x => x.PasswordSignInAsync(model.Username, model.Password, false, true))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        _mockUserManager
            .Setup(x => x.FindByNameAsync(model.Username))
            .ReturnsAsync(new ApplicationUser { Id = "testId", UserName = model.Username });

        var controller = new AccountController(
            _mockInteraction.Object,
            _mockClientStore.Object,
            _mockSchemeProvider.Object,
            _mockEvents.Object,
            _mockUserManager.Object,
            _mockSignInManager.Object,
            _mockRoleManager.Object,
            _mockLogger.Object,
            _mockRemoteHostService.Object,
            _mockIpBlockingService.Object
        );

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = mockHttpContext.Object
        };
        controller.Url = mockUrlHelper.Object;

        // Act
        var result = await controller.Register(model, "/");

        // Assert
        var redirectResult = Assert.IsType<RedirectResult>(result);
        Assert.Equal("/", redirectResult.Url);

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Registering new user")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ),
            Times.Once);

        _mockUserManager.Verify(x => x.CreateAsync(
                It.Is<ApplicationUser>(u =>
                    u.UserName == model.Username &&
                    u.Email == model.Email),
                model.Password),
            Times.Once);

        _mockUserManager.Verify(x => x.AddToRoleAsync(
                It.IsAny<ApplicationUser>(),
                "Customer"),
            Times.Once);
    }

    [Fact]
    public async Task Login_WithBlockedIp_ReturnsForbidden()
    {
        // Arrange
        var blockedIp = IPAddress.Parse("192.168.1.1");
        var model = new LoginInputModel
        {
            Username = "testuser",
            Password = "password"
        };

        _mockRemoteHostService
            .Setup(x => x.GetRemoteHostIpAddressUsingRemoteIpAddress(It.IsAny<HttpContext>()))
            .Returns(blockedIp);

        _mockIpBlockingService
            .Setup(x => x.IsBlocked(blockedIp))
            .Returns(true);

        var controller = new AccountController(
            _mockInteraction.Object,
            _mockClientStore.Object,
            _mockSchemeProvider.Object,
            _mockEvents.Object,
            _mockUserManager.Object,
            _mockSignInManager.Object,
            _mockRoleManager.Object,
            _mockLogger.Object,
            _mockRemoteHostService.Object,
            _mockIpBlockingService.Object
        );

        // Act
        var result = await controller.Login(model, "login");

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status403Forbidden, objectResult.StatusCode);
        Assert.Equal("Your IP address is blocked.", objectResult.Value);

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Blocked IP attempted to login")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ),
            Times.Once);
    }

    [Fact]
    public async Task Login_WithValidCredentials_LogsSuccessAndRedirects()
    {
        // Arrange
        var validIp = IPAddress.Parse("127.0.0.1");
        var model = new LoginInputModel
        {
            Username = "testuser",
            Password = "password",
            ReturnUrl = "/"
        };

        var user = new ApplicationUser
        {
            UserName = model.Username,
            Email = "test@test.com"
        };

        var mockHttpContext = new Mock<HttpContext>();
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);

        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper.Setup(x => x.IsLocalUrl(It.IsAny<string>())).Returns(true);

        _mockRemoteHostService
            .Setup(x => x.GetRemoteHostIpAddressUsingRemoteIpAddress(It.IsAny<HttpContext>()))
            .Returns(validIp);

        _mockIpBlockingService
            .Setup(x => x.IsBlocked(validIp))
            .Returns(false);

        _mockSignInManager
            .Setup(x => x.PasswordSignInAsync(model.Username, model.Password, false, false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        _mockUserManager
            .Setup(x => x.FindByNameAsync(model.Username))
            .ReturnsAsync(user);

        var controller = new AccountController(
            _mockInteraction.Object,
            _mockClientStore.Object,
            _mockSchemeProvider.Object,
            _mockEvents.Object,
            _mockUserManager.Object,
            _mockSignInManager.Object,
            _mockRoleManager.Object,
            _mockLogger.Object,
            _mockRemoteHostService.Object,
            _mockIpBlockingService.Object
        );

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = mockHttpContext.Object
        };
        controller.Url = mockUrlHelper.Object;

        // Act
        var result = await controller.Login(model, "login");

        // Assert
        var redirectResult = Assert.IsType<RedirectResult>(result);
        Assert.Equal("/", redirectResult.Url);

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Logging in user")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ),
            Times.Once);
    }
}