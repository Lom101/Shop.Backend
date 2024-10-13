using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shop.WebAPI.Controllers;
using Shop.WebAPI.Dtos.Brand.Request;
using Shop.WebAPI.Dtos.Brand.Response;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;

namespace Shop.Tests.Controllers;

public class BrandControllerTests
{
    private readonly Mock<IBrandRepository> _mockBrandRepository;
    private readonly IMapper _mapper;
    private readonly BrandController _controller;

    public BrandControllerTests()
    {
        _mockBrandRepository = new Mock<IBrandRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Brand, GetBrandResponse>();
            cfg.CreateMap<CreateBrandRequest, Brand>();
            cfg.CreateMap<UpdateBrandRequest, Brand>();
        });
        _mapper = config.CreateMapper();

        _controller = new BrandController(_mockBrandRepository.Object, _mapper);
    }

    [Fact]
    public async Task GetAllBrands_ReturnsOkResult_WithListOfBrands()
    {
        // Arrange
        var brands = new List<Brand>
        {
            new Brand { Id = 1, Name = "Brand1" },
            new Brand { Id = 2, Name = "Brand2" }
        };
        _mockBrandRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(brands);

        // Act
        var result = await _controller.GetAllBrands();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsAssignableFrom<IEnumerable<GetBrandResponse>>(okResult.Value);
        Assert.Equal(2, response.Count());
    }

    [Fact]
    public async Task GetBrandById_ReturnsNotFound_WhenBrandDoesNotExist()
    {
        // Arrange
        int id = 1;
        _mockBrandRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Brand)null);

        // Act
        var result = await _controller.GetBrandById(id);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task CreateBrand_ReturnsCreatedAtAction_WithResponse()
    {
        // Arrange
        var request = new CreateBrandRequest { Name = "New Brand" };
        var brand = new Brand { Id = 1, Name = "New Brand" };
        _mockBrandRepository.Setup(repo => repo.AddAsync(It.IsAny<Brand>())).ReturnsAsync(true);
        _mockBrandRepository.Setup(repo => repo.GetByIdAsync(brand.Id)).ReturnsAsync(brand);

        // Act
        var result = await _controller.CreateBrand(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var response = Assert.IsType<GetBrandResponse>(createdResult.Value);
        Assert.Equal(brand.Name, response.Name);
    }

    [Fact]
    public async Task UpdateBrand_ReturnsOkResult_WhenBrandIsUpdated()
    {
        // Arrange
        int id = 1;
        var request = new UpdateBrandRequest { Id = id, Name = "Updated Brand" };
        var brand = new Brand { Id = id, Name = "Old Brand" };
        _mockBrandRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(brand);
        _mockBrandRepository.Setup(repo => repo.UpdateAsync(brand)).ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateBrand(id, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<GetBrandResponse>(okResult.Value);
        Assert.Equal(request.Name, response.Name);
    }

    [Fact]
    public async Task DeleteBrand_ReturnsNoContent_WhenBrandIsDeleted()
    {
        // Arrange
        int id = 1;
        var brand = new Brand { Id = id, Name = "Brand to Delete" };
        _mockBrandRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(brand);
        _mockBrandRepository.Setup(repo => repo.DeleteAsync(id)).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteBrand(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteBrand_ReturnsNotFound_WhenBrandDoesNotExist()
    {
        // Arrange
        int id = 1;
        _mockBrandRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Brand)null);

        // Act
        var result = await _controller.DeleteBrand(id);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}