using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shop.WebAPI.Controllers;
using Shop.WebAPI.Dtos.Model.Request;
using Shop.WebAPI.Dtos.Model.Response;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;

namespace Shop.Tests.Controllers;

public class ModelControllerTests
{
    private readonly Mock<IModelRepository> _mockModelRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ModelController _controller;

    public ModelControllerTests()
    {
        _mockModelRepository = new Mock<IModelRepository>();
        _mockMapper = new Mock<IMapper>();
        _controller = new ModelController(_mockModelRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetModelById_ReturnsOk_WhenModelExists()
    {
        var model = new Model { Id = 1, Price = 100 };
        var modelResponse = new GetModelResponse { Id = 1, Price = 100 };

        _mockModelRepository.Setup(repo => repo.GetModelByIdAsync(1)).ReturnsAsync(model);
        _mockMapper.Setup(m => m.Map<GetModelResponse>(model)).Returns(modelResponse);

        var result = await _controller.GetModelById(1) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(modelResponse, result.Value);
    }

    [Fact]
    public async Task GetAllModels_ReturnsOk_WithModelList()
    {
        var models = new List<Model> { new Model { Id = 1, Price = 100 }, new Model { Id = 2, Price = 200 } };
        var modelResponses = new List<GetModelResponse>
        {
            new GetModelResponse { Id = 1, Price = 100 },
            new GetModelResponse { Id = 2, Price = 200 }
        };

        _mockModelRepository.Setup(repo => repo.GetAllModelsAsync()).ReturnsAsync(models);
        _mockMapper.Setup(m => m.Map<IEnumerable<GetModelResponse>>(models)).Returns(modelResponses);

        var result = await _controller.GetAllModels() as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(modelResponses, result.Value);
    }

    [Fact]
    public async Task CreateModel_ReturnsCreatedAtAction_WhenModelIsCreated()
    {
        var createRequest = new CreateModelRequest { Price = 150, ProductId = 1, ColorId = 1, SizeIds = new List<int> { 1, 2 } };
        var model = new Model { Id = 1, Price = 150 };
        _mockMapper.Setup(m => m.Map<Model>(createRequest)).Returns(model);
        _mockModelRepository.Setup(repo => repo.AddModelAsync(model)).ReturnsAsync(1);

        var result = await _controller.CreateModel(createRequest) as CreatedAtActionResult;

        Assert.NotNull(result);
        Assert.Equal(201, result.StatusCode);
    }

    [Fact]
    public async Task UpdateModel_ReturnsNoContent_WhenUpdateIsSuccessful()
    {
        var updateRequest = new UpdateModelRequest { Id = 1, Price = 200, ProductId = 1, ColorId = 1 };
        var model = new Model { Id = 1, Price = 200 };

        _mockMapper.Setup(m => m.Map<Model>(updateRequest)).Returns(model);
        _mockModelRepository.Setup(repo => repo.UpdateModelAsync(model)).ReturnsAsync(true);

        var result = await _controller.UpdateModel(1, updateRequest) as NoContentResult;

        Assert.NotNull(result);
        Assert.Equal(204, result.StatusCode);
    }

    [Fact]
    public async Task DeleteModel_ReturnsNoContent_WhenDeleteIsSuccessful()
    {
        _mockModelRepository.Setup(repo => repo.DeleteModelAsync(1)).ReturnsAsync(true);

        var result = await _controller.DeleteModel(1) as NoContentResult;

        Assert.NotNull(result);
        Assert.Equal(204, result.StatusCode);
    }
}