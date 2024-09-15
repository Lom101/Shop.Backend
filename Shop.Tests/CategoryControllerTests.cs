using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shop.WebAPI.Controllers;
using Shop.WebAPI.Dtos.Category.Requests;
using Shop.WebAPI.Dtos.Category.Responses;
using Shop.WebAPI.Services.Interfaces;
using Xunit;

namespace Shop.Tests
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryService> _mockCategoryService;
        private readonly CategoryController _controller;

        public CategoryControllerTests()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _controller = new CategoryController(_mockCategoryService.Object);
        }

        [Fact]
        public async Task GetAllCategories_ShouldReturnOkResult_WithListOfCategories()
        {
            // Arrange
            var mockCategories = new List<GetCategoryResponse>
            {
                new GetCategoryResponse { Id = 1, Name = "Category 1" },
                new GetCategoryResponse { Id = 2, Name = "Category 2" }
            };
            _mockCategoryService.Setup(s => s.GetAllCategoriesAsync())
                .ReturnsAsync(mockCategories);

            // Act
            var result = await _controller.GetAllCategories();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var categories = okResult.Value.Should().BeAssignableTo<IEnumerable<GetCategoryResponse>>().Subject;
            categories.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetCategoryById_ShouldReturnOkResult_WithCategory()
        {
            // Arrange
            var mockCategory = new GetCategoryResponse { Id = 1, Name = "Category 1" };
            _mockCategoryService.Setup(s => s.GetCategoryByIdAsync(1))
                .ReturnsAsync(mockCategory);

            // Act
            var result = await _controller.GetCategoryById(1);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var category = okResult.Value.Should().BeAssignableTo<GetCategoryResponse>().Subject;
            category.Id.Should().Be(1);
            category.Name.Should().Be("Category 1");
        }

        [Fact]
        public async Task GetCategoryById_ShouldReturnNotFound_WhenCategoryDoesNotExist()
        {
            // Arrange
            _mockCategoryService.Setup(s => s.GetCategoryByIdAsync(1))
                .ReturnsAsync((GetCategoryResponse)null);

            // Act
            var result = await _controller.GetCategoryById(1);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task AddCategory_ShouldReturnCreatedAtActionResult()
        {
            // Arrange
            var createCategoryRequest = new CreateCategoryRequest { Name = "New Category" };
            _mockCategoryService.Setup(s => s.AddCategoryAsync(createCategoryRequest))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.AddCategory(createCategoryRequest);

            // Assert
            var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.ActionName.Should().Be(nameof(_controller.GetCategoryById));
            createdResult.RouteValues["id"].Should().Be(1);
        }

        [Fact]
        public async Task UpdateCategory_ShouldReturnNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var updateCategoryRequest = new UpdateCategoryRequest { Id = 1, Name = "Updated Category" };
            _mockCategoryService.Setup(s => s.UpdateCategoryAsync(updateCategoryRequest))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateCategory(1, updateCategoryRequest);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdateCategory_ShouldReturnNotFound_WhenCategoryDoesNotExist()
        {
            // Arrange
            var updateCategoryRequest = new UpdateCategoryRequest { Id = 1, Name = "Updated Category" };
            _mockCategoryService.Setup(s => s.UpdateCategoryAsync(updateCategoryRequest))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateCategory(1, updateCategoryRequest);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateCategory_ShouldReturnBadRequest_WhenIdMismatch()
        {
            // Arrange
            var updateCategoryRequest = new UpdateCategoryRequest { Id = 2, Name = "Updated Category" };

            // Act
            var result = await _controller.UpdateCategory(1, updateCategoryRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be("category ID mismatch");
        }

        [Fact]
        public async Task DeleteCategory_ShouldReturnNoContent_WhenDeleteIsSuccessful()
        {
            // Arrange
            _mockCategoryService.Setup(s => s.DeleteCategoryAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteCategory(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteCategory_ShouldReturnNotFound_WhenCategoryDoesNotExist()
        {
            // Arrange
            _mockCategoryService.Setup(s => s.DeleteCategoryAsync(1))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteCategory(1);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
