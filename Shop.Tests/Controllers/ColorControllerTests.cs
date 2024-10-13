using Microsoft.AspNetCore.Mvc;
using Moq;
using Shop.WebAPI.Controllers;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Shop.Tests.Controllers
{
    public class ColorControllerTests
    {
        private readonly Mock<IColorRepository> _colorRepositoryMock;
        private readonly ColorController _controller;

        public ColorControllerTests()
        {
            _colorRepositoryMock = new Mock<IColorRepository>();
            _controller = new ColorController(_colorRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllColors_ReturnsOkResult_WithColorList()
        {
            // Arrange
            var colors = new List<Color>
            {
                new Color { Id = 1, Name = "Red" },
                new Color { Id = 2, Name = "Blue" }
            };
            _colorRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(colors);

            // Act
            var result = await _controller.GetAllColors();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedColors = Assert.IsAssignableFrom<IEnumerable<Color>>(okResult.Value);
            Assert.Equal(2, returnedColors.Count());
        }

        [Fact]
        public async Task GetColorById_ReturnsOkResult_WhenColorExists()
        {
            // Arrange
            var color = new Color { Id = 1, Name = "Red" };
            _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(color);

            // Act
            var result = await _controller.GetColorById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedColor = Assert.IsType<Color>(okResult.Value);
            Assert.Equal("Red", returnedColor.Name);
        }

        [Fact]
        public async Task GetColorById_ReturnsNotFound_WhenColorDoesNotExist()
        {
            // Arrange
            _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((Color)null);

            // Act
            var result = await _controller.GetColorById(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
