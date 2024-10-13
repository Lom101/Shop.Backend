using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shop.WebAPI.Controllers;
using Shop.WebAPI.Dtos.Category.Requests;
using Shop.WebAPI.Dtos.Category.Responses;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;

namespace Shop.Tests.Controllers
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly IMapper _mapper;
        private readonly CategoryController _controller;

        public CategoryControllerTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, GetCategoryResponse>();
                cfg.CreateMap<CreateCategoryRequest, Category>();
                cfg.CreateMap<UpdateCategoryRequest, Category>();
            });
            _mapper = config.CreateMapper();
            _controller = new CategoryController(_categoryRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllCategories_ReturnsOkResult_WithListOfCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category1" },
                new Category { Id = 2, Name = "Category2" }
            };
            _categoryRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);

            // Act
            var result = await _controller.GetAllCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsAssignableFrom<IEnumerable<GetCategoryResponse>>(okResult.Value);
            Assert.Equal(2, response.Count());
        }

        [Fact]
        public async Task GetCategoryById_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            // Arrange
            int categoryId = 1;
            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync((Category)null);

            // Act
            var result = await _controller.GetCategoryById(categoryId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Категория с ID {categoryId} не найдена.", notFoundResult.Value);
        }

        [Fact]
        public async Task AddCategory_ReturnsCreatedAtAction_WhenCategoryIsSuccessfullyAdded()
        {
            // Arrange
            var createRequest = new CreateCategoryRequest { Name = "NewCategory" };
            var newCategory = new Category { Id = 1, Name = "NewCategory" }; // Simulate that the ID will be set after adding
    
            // Mock the repository methods
            _categoryRepositoryMock.Setup(repo => repo.GetByNameAsync(createRequest.Name)).ReturnsAsync((Category)null);
            _categoryRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Category>())).ReturnsAsync(true); // Simulate successful addition

            // Act
            var result = await _controller.AddCategory(createRequest);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var response = Assert.IsType<GetCategoryResponse>(createdResult.Value);
            Assert.Equal(newCategory.Name, response.Name);
        }
        

        [Fact]
        public async Task UpdateCategory_ReturnsOk_WhenCategoryIsUpdatedSuccessfully()
        {
            // Arrange
            int categoryId = 1;
            var updateRequest = new UpdateCategoryRequest { Id = categoryId, Name = "UpdatedCategory" };
            var existingCategory = new Category { Id = categoryId, Name = "OldCategory" };

            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(existingCategory);
            _categoryRepositoryMock.Setup(repo => repo.UpdateAsync(existingCategory)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateCategory(categoryId, updateRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GetCategoryResponse>(okResult.Value);
            Assert.Equal(updateRequest.Name, response.Name);
        }

        [Fact]
        public async Task DeleteCategory_ReturnsNoContent_WhenCategoryIsDeletedSuccessfully()
        {
            // Arrange
            int categoryId = 1;
            var existingCategory = new Category { Id = categoryId, Name = "OldCategory" };

            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(existingCategory);
            _categoryRepositoryMock.Setup(repo => repo.DeleteAsync(categoryId)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteCategory(categoryId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
