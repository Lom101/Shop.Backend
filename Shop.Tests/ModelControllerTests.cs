using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shop.WebAPI.Controllers;
using Shop.WebAPI.Dtos.Model.Request;
using Shop.WebAPI.Dtos.Model.Response;
using Shop.WebAPI.Services.Interfaces;
using Xunit;

namespace Shop.Tests
{
    public class ModelControllerTests
    {
        private readonly Mock<IModelService> _mockModelService;
        private readonly ModelController _controller;

        public ModelControllerTests()
        {
            _mockModelService = new Mock<IModelService>();
            _controller = new ModelController(_mockModelService.Object);
        }

        [Fact]
        public async Task GetAllModels_ShouldReturnOkResult_WithListOfModels()
        {
            // Arrange
            var mockModels = new List<GetModelResponse>
            {
                new GetModelResponse { Id = 1, Price = 100.00 },
                new GetModelResponse { Id = 2, Price = 200.00 }
            };
            _mockModelService.Setup(s => s.GetAllModelsAsync())
                .ReturnsAsync(mockModels);

            // Act
            var result = await _controller.GetAllModels();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var models = okResult.Value.Should().BeAssignableTo<IEnumerable<GetModelResponse>>().Subject;
            models.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetModelById_ShouldReturnOkResult_WithModel()
        {
            // Arrange
            var mockModel = new GetModelResponse { Id = 1, Price = 100.00 };
            _mockModelService.Setup(s => s.GetModelByIdAsync(1))
                .ReturnsAsync(mockModel);

            // Act
            var result = await _controller.GetModelById(1);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var model = okResult.Value.Should().BeAssignableTo<GetModelResponse>().Subject;
            model.Id.Should().Be(1);
            model.Price.Should().Be(100.00);
        }

        [Fact]
        public async Task GetModelById_ShouldReturnNotFound_WhenModelDoesNotExist()
        {
            // Arrange
            _mockModelService.Setup(s => s.GetModelByIdAsync(1))
                .ReturnsAsync((GetModelResponse)null);

            // Act
            var result = await _controller.GetModelById(1);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task CreateModel_ShouldReturnCreatedAtActionResult()
        {
            // Arrange
            var createModelRequest = new CreateModelRequest { Price = 100.00, ProductId = 1, ColorId = 1, SizeIds = new List<int> { 1, 2 } };
            _mockModelService.Setup(s => s.AddModelAsync(createModelRequest))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.CreateModel(createModelRequest);

            // Assert
            var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.ActionName.Should().Be(nameof(_controller.GetModelById));
            createdResult.RouteValues["id"].Should().Be(1);
        }

        [Fact]
        public async Task UpdateModel_ShouldReturnNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var updateModelRequest = new UpdateModelRequest { Id = 1, Price = 150.00, ProductId = 1, ColorId = 1, SizeIds = new List<int> { 1, 2 } };
            _mockModelService.Setup(s => s.UpdateModelAsync(updateModelRequest))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateModel(1, updateModelRequest);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdateModel_ShouldReturnNotFound_WhenModelDoesNotExist()
        {
            // Arrange
            var updateModelRequest = new UpdateModelRequest { Id = 1, Price = 150.00, ProductId = 1, ColorId = 1, SizeIds = new List<int> { 1, 2 } };
            _mockModelService.Setup(s => s.UpdateModelAsync(updateModelRequest))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateModel(1, updateModelRequest);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateModel_ShouldReturnBadRequest_WhenIdMismatch()
        {
            // Arrange
            var updateModelRequest = new UpdateModelRequest { Id = 2, Price = 150.00, ProductId = 1, ColorId = 1, SizeIds = new List<int> { 1, 2 } };

            // Act
            var result = await _controller.UpdateModel(1, updateModelRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be("Model ID mismatch");
        }

        [Fact]
        public async Task DeleteModel_ShouldReturnNoContent_WhenDeleteIsSuccessful()
        {
            // Arrange
            _mockModelService.Setup(s => s.DeleteModelAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteModel(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteModel_ShouldReturnNotFound_WhenModelDoesNotExist()
        {
            // Arrange
            _mockModelService.Setup(s => s.DeleteModelAsync(1))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteModel(1);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
