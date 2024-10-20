using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shop.WebAPI.Controllers;
using Shop.WebAPI.Dtos.Address.Responses;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;

namespace Shop.Tests.Controllers;

public class AddressControllerTests
{
    private readonly Mock<IAddressRepository> _addressRepositoryMock;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AddressController _controller;

    public AddressControllerTests()
    {
        _addressRepositoryMock = new Mock<IAddressRepository>();
        _mapperMock = new Mock<IMapper>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

        _controller = new AddressController(
            _addressRepositoryMock.Object, 
            _userManagerMock.Object, 
            _mapperMock.Object);
    }

    [Fact]
    public async Task GetAddressesByUserId_ReturnsOk_WhenAddressesExist()
    {
        // Arrange
        var userId = "user123";
        var user = new ApplicationUser { Id = userId };
        var addresses = new List<Address>
        {
            new Address { Id = 1, UserId = userId, AddressName = "Home" },
            new Address { Id = 2, UserId = userId, AddressName = "Work" }
        };
        var mappedAddresses = new List<GetAddressResponse>
        {
            new GetAddressResponse { Id = 1, AddressName = "Home" },
            new GetAddressResponse { Id = 2, AddressName = "Work" }
        };

        _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);
        _addressRepositoryMock.Setup(x => x.GetByUserIdAsync(userId)).ReturnsAsync(addresses);
        _mapperMock.Setup(x => x.Map<IEnumerable<GetAddressResponse>>(addresses)).Returns(mappedAddresses);

        // Act
        var result = await _controller.GetAddressesByUserId(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedAddresses = Assert.IsType<List<GetAddressResponse>>(okResult.Value);
        Assert.Equal(2, returnedAddresses.Count);
    }

    [Fact]
    public async Task GetAddressesByUserId_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = "user123";
        _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync((ApplicationUser)null);

        // Act
        var result = await _controller.GetAddressesByUserId(userId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var message = notFoundResult.Value as string;
        Assert.Equal("Пользователь не найден.", message);
    }

    [Fact]
    public async Task GetMyAddresses_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity())
            }
        };

        // Act
        var result = await _controller.GetMyAddresses();

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        var message = unauthorizedResult.Value as string;
        Assert.Equal("Пользователь не авторизован.", message);
    }

    [Fact]
    public async Task GetMyAddresses_ReturnsOk_WhenAddressesExist()
    {
        // Arrange
        var userId = "user123";
        var addresses = new List<Address>
        {
            new Address { Id = 1, UserId = userId, AddressName = "Home" },
            new Address { Id = 2, UserId = userId, AddressName = "Work" }
        };
        var mappedAddresses = new List<GetAddressResponse>
        {
            new GetAddressResponse { Id = 1, AddressName = "Home" },
            new GetAddressResponse { Id = 2, AddressName = "Work" }
        };

        _addressRepositoryMock.Setup(x => x.GetByUserIdAsync(userId)).ReturnsAsync(addresses);
        _mapperMock.Setup(x => x.Map<IEnumerable<GetAddressResponse>>(addresses)).Returns(mappedAddresses);

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        }, "mock"));
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };

        // Act
        var result = await _controller.GetMyAddresses();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedAddresses = Assert.IsType<List<GetAddressResponse>>(okResult.Value);
        Assert.Equal(2, returnedAddresses.Count);
    }

    [Fact]
    public async Task GetMyAddresses_ReturnsNotFound_WhenNoAddressesExist()
    {
        // Arrange
        var userId = "user123";
        _addressRepositoryMock.Setup(x => x.GetByUserIdAsync(userId)).ReturnsAsync(new List<Address>());

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        }, "mock"));
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };

        // Act
        var result = await _controller.GetMyAddresses();

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var message = notFoundResult.Value as string;
        Assert.Equal("Для текущего пользователя адреса не найдены.", message);
    }
}