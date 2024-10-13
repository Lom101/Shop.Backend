using Microsoft.AspNetCore.Mvc;
using Moq;
using Shop.WebAPI.Controllers;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;

namespace Shop.Tests.Controllers;

public class SizeControllerTests
{
    [Fact]
    public async Task GetAllSizes_ShouldReturnOkResult_WithListOfSizes()
    {
        // Arrange
        var mockRepository = new Mock<ISizeRepository>();
        mockRepository.Setup(repo => repo.GetAllSizesAsync())
            .ReturnsAsync(new List<Size>
            {
                new Size { Id = 1, Name = "Small" },
                new Size { Id = 2, Name = "Medium" }
            });

        var controller = new SizeController(mockRepository.Object);

        // Act
        var result = await controller.GetAllSizes();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var sizes = Assert.IsType<List<Size>>(okResult.Value);
        Assert.Equal(2, sizes.Count);
    }

    [Fact]
    public async Task GetSizeById_ShouldReturnOkResult_WhenSizeExists()
    {
        // Arrange
        var mockRepository = new Mock<ISizeRepository>();
        mockRepository.Setup(repo => repo.GetSizeByIdAsync(1))
            .ReturnsAsync(new Size { Id = 1, Name = "Large" });

        var controller = new SizeController(mockRepository.Object);

        // Act
        var result = await controller.GetSizeById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var size = Assert.IsType<Size>(okResult.Value);
        Assert.Equal("Large", size.Name);
    }

    [Fact]
    public async Task GetSizeById_ShouldReturnNotFound_WhenSizeDoesNotExist()
    {
        // Arrange
        var mockRepository = new Mock<ISizeRepository>();
        mockRepository.Setup(repo => repo.GetSizeByIdAsync(1))
            .ReturnsAsync((Size)null);

        var controller = new SizeController(mockRepository.Object);

        // Act
        var result = await controller.GetSizeById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}