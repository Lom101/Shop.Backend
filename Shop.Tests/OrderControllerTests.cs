using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shop.WebAPI.Controllers;
using Shop.WebAPI.Dtos.Order.Requests;
using Shop.WebAPI.Dtos.Order.Responses;
using Shop.WebAPI.Enums;
using Shop.WebAPI.Services.Interfaces;
using Xunit;

namespace Shop.Tests
{
    public class OrderControllerTests
    {
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _mockOrderService = new Mock<IOrderService>();
            _controller = new OrderController(_mockOrderService.Object);
        }

        [Fact]
        public async Task GetAllOrders_ShouldReturnOkResult_WithListOfOrders()
        {
            // Arrange
            var mockOrders = new List<GetOrderResponse>
            {
                new GetOrderResponse { Id = 1, UserId = "user1", OrderDate = DateTime.UtcNow, Status = OrderStatus.Pending.ToString(), TotalAmount = 100.00m },
                new GetOrderResponse { Id = 2, UserId = "user2", OrderDate = DateTime.UtcNow, Status = OrderStatus.Shipped.ToString(), TotalAmount = 200.00m }
            };
            _mockOrderService.Setup(s => s.GetAllOrdersAsync())
                .ReturnsAsync(mockOrders);

            // Act
            var result = await _controller.GetAllOrders();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var orders = okResult.Value.Should().BeAssignableTo<IEnumerable<GetOrderResponse>>().Subject;
            orders.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetOrderById_ShouldReturnOkResult_WithOrder()
        {
            // Arrange
            var mockOrder = new GetOrderResponse 
            { 
                Id = 1, 
                UserId = "user1", 
                OrderDate = DateTime.UtcNow, 
                Status = OrderStatus.Pending.ToString(), 
                TotalAmount = 100.00m 
            };
            _mockOrderService.Setup(s => s.GetOrderByIdAsync(1))
                .ReturnsAsync(mockOrder);

            // Act
            var result = await _controller.GetOrderById(1);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var order = okResult.Value.Should().BeAssignableTo<GetOrderResponse>().Subject;
            order.Id.Should().Be(1);
            order.TotalAmount.Should().Be(100.00m);
        }

        [Fact]
        public async Task GetOrderById_ShouldReturnNotFound_WhenOrderDoesNotExist()
        {
            // Arrange
            _mockOrderService.Setup(s => s.GetOrderByIdAsync(1))
                .ReturnsAsync((GetOrderResponse)null);

            // Act
            var result = await _controller.GetOrderById(1);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task AddOrder_ShouldReturnCreatedAtActionResult()
        {
            // Arrange
            var createOrderRequest = new CreateOrderRequest 
            { 
                UserId = "user1", 
                OrderDate = DateTime.UtcNow, 
                Status = OrderStatus.Pending, 
                TotalAmount = 100.00m 
            };
            _mockOrderService.Setup(s => s.AddOrderAsync(createOrderRequest))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.AddOrder(createOrderRequest);

            // Assert
            var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.ActionName.Should().Be(nameof(_controller.GetOrderById));
            createdResult.RouteValues["id"].Should().Be(1);
        }

        [Fact]
        public async Task UpdateOrder_ShouldReturnNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var updateOrderRequest = new UpdateOrderRequest 
            { 
                Id = 1, 
                UserId = "user1", 
                OrderDate = DateTime.UtcNow, 
                Status = OrderStatus.Shipped, 
                TotalAmount = 150.00m 
            };
            _mockOrderService.Setup(s => s.UpdateOrderAsync(updateOrderRequest))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateOrder(1, updateOrderRequest);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdateOrder_ShouldReturnNotFound_WhenOrderDoesNotExist()
        {
            // Arrange
            var updateOrderRequest = new UpdateOrderRequest 
            { 
                Id = 1, 
                UserId = "user1", 
                OrderDate = DateTime.UtcNow, 
                Status = OrderStatus.Shipped, 
                TotalAmount = 150.00m 
            };
            _mockOrderService.Setup(s => s.UpdateOrderAsync(updateOrderRequest))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateOrder(1, updateOrderRequest);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateOrder_ShouldReturnBadRequest_WhenIdMismatch()
        {
            // Arrange
            var updateOrderRequest = new UpdateOrderRequest 
            { 
                Id = 2, 
                UserId = "user1", 
                OrderDate = DateTime.UtcNow, 
                Status = OrderStatus.Shipped, 
                TotalAmount = 150.00m 
            };

            // Act
            var result = await _controller.UpdateOrder(1, updateOrderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be("order ID mismatch");
        }

        [Fact]
        public async Task DeleteOrder_ShouldReturnNoContent_WhenDeleteIsSuccessful()
        {
            // Arrange
            _mockOrderService.Setup(s => s.DeleteOrderAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteOrder(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteOrder_ShouldReturnNotFound_WhenOrderDoesNotExist()
        {
            // Arrange
            _mockOrderService.Setup(s => s.DeleteOrderAsync(1))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteOrder(1);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
