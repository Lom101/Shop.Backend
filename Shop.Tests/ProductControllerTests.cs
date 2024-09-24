using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shop.WebAPI.Controllers;
using Shop.WebAPI.Dtos;
using Shop.WebAPI.Dtos.Brand;
using Shop.WebAPI.Dtos.Category;
using Shop.WebAPI.Dtos.Product.Requests;
using Shop.WebAPI.Dtos.Product.Responses;
using Shop.WebAPI.Services.Interfaces;
using Xunit;

namespace Shop.Tests
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _controller = new ProductController(_mockProductService.Object);
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnOkResult_WithListOfProducts()
        {
            // Arrange
            var mockProducts = new List<GetProductResponse>
            {
                new GetProductResponse 
                { 
                    Id = 1, 
                    Name = "Product 1", 
                    Description = "Description 1", 
                    Created = DateTime.Now,
                    CategoryId = 1, 
                    Category = new CategoryDto { Id = 1, Name = "Category 1" },
                    BrandId = 1, 
                    Brand = new BrandDto { Id = 1, Name = "Brand 1" },
                    Models = new List<ModelDto> { new ModelDto { Id = 1, Price = 10 } }
                },
                new GetProductResponse 
                { 
                    Id = 2, 
                    Name = "Product 2", 
                    Description = "Description 2", 
                    Created = DateTime.Now,
                    CategoryId = 2, 
                    Category = new CategoryDto { Id = 2, Name = "Category 2" },
                    BrandId = 2, 
                    Brand = new BrandDto { Id = 2, Name = "Brand 2" },
                    Models = new List<ModelDto> { new ModelDto { Id = 2, Price = 20 } }
                }
            };
            _mockProductService.Setup(s => s.GetAllProductsAsync())
                .ReturnsAsync(mockProducts);

            // Act
            var result = await _controller.GetAllProducts();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var products = okResult.Value.Should().BeAssignableTo<IEnumerable<GetProductResponse>>().Subject;
            products.Should().HaveCount(2);
        }


        [Fact]
        public async Task GetProductById_ShouldReturnOkResult_WithProduct()
        {
            // Arrange
            var mockProduct = new GetProductResponse
            {
                Id = 1,
                Name = "Product 1",
                Description = "Description 1",
                Created = DateTime.Now,
                CategoryId = 1,
                Category = new CategoryDto { Id = 1, Name = "Category 1" },
                BrandId = 1,
                Brand = new BrandDto { Id = 1, Name = "Brand 1" },
                Models = new List<ModelDto> { new ModelDto { Id = 1, Price = 10} }
            };
            _mockProductService.Setup(s => s.GetProductByIdAsync(1))
                .ReturnsAsync(mockProduct);

            // Act
            var result = await _controller.GetProductById(1);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var product = okResult.Value.Should().BeAssignableTo<GetProductResponse>().Subject;
            product.Id.Should().Be(1);
            product.Name.Should().Be("Product 1");
            product.Brand.Name.Should().Be("Brand 1");  
            product.Category.Name.Should().Be("Category 1");
            product.Models.Should().HaveCount(1);
        }

        
        [Fact]
        public async Task AddProduct_ShouldReturnCreatedAtActionResult()
        {
            // Arrange
            var createProductRequest = new CreateProductRequest
            {
                Name = "Product 1",
                Description = "Description 1",
                Created = DateTime.Now,
                CategoryId = 1,
                BrandId = 1,
                Models = new List<ModelDto> { new ModelDto { Id = 1, Price = 10 } }
            };
            _mockProductService.Setup(s => s.AddProductAsync(createProductRequest))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.AddProduct(createProductRequest);

            // Assert
            var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.ActionName.Should().Be(nameof(_controller.GetProductById));
            createdResult.RouteValues?["id"].Should().Be(1);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _mockProductService.Setup(s => s.GetProductByIdAsync(1))
                .ReturnsAsync((GetProductResponse)null);

            // Act
            var result = await _controller.GetProductById(1);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }


        [Fact]
        public async Task UpdateProduct_ShouldReturnNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var updateProductRequest = new UpdateProductRequest
            {
                Id = 1,
                Name = "Updated Product",
                Description = "Updated Description",
                Created = DateTime.Now,
                CategoryId = 2,
                BrandId = 2,
                Models = new List<ModelDto> { new ModelDto { Id = 1, Price = 10 } }
            };
            _mockProductService.Setup(s => s.UpdateProductAsync(updateProductRequest))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateProduct(1, updateProductRequest);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var updateProductRequest = new UpdateProductRequest
            {
                Id = 1,
                Name = "Updated Product",
                Description = "Updated Description",
                Created = DateTime.Now,
                CategoryId = 2,
                BrandId = 1,
                Models = new List<ModelDto> { new ModelDto { Id = 1, Price = 10 } }
            };
            _mockProductService.Setup(s => s.UpdateProductAsync(updateProductRequest))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateProduct(1, updateProductRequest);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnBadRequest_WhenIdMismatch()
        {
            // Arrange
            var updateProductRequest = new UpdateProductRequest
            {
                Id = 2,
                Name = "Updated Product",
                Description = "Updated Description",
                Created = DateTime.Now,
                CategoryId = 2,
                BrandId = 1,
                Models = new List<ModelDto> { new ModelDto { Id = 1, Price = 10 } }
            };

            // Act
            var result = await _controller.UpdateProduct(1, updateProductRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be("product ID mismatch");
        }


        [Fact]
        public async Task DeleteProduct_ShouldReturnNoContent_WhenDeleteIsSuccessful()
        {
            // Arrange
            _mockProductService.Setup(s => s.DeleteProductAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteProduct(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _mockProductService.Setup(s => s.DeleteProductAsync(1))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteProduct(1);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
