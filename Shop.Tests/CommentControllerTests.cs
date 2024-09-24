using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shop.WebAPI.Controllers;
using Shop.WebAPI.Dtos.Comment.Requests;
using Shop.WebAPI.Dtos.Comment.Responses;
using Shop.WebAPI.Services.Interfaces;
using Xunit;

namespace Shop.Tests
{
    public class CommentControllerTests
    {
        private readonly Mock<ICommentService> _mockCommentService;
        private readonly CommentController _controller;

        public CommentControllerTests()
        {
            _mockCommentService = new Mock<ICommentService>();
            _controller = new CommentController(_mockCommentService.Object);
        }

        [Fact]
        public async Task GetAllComments_ShouldReturnOkResult_WithListOfComments()
        {
            // Arrange
            var mockComments = new List<GetCommentResponse>
            {
                new GetCommentResponse { Id = 1, Text = "Great product!", Created = DateTime.UtcNow, ProductId = 1, UserId = "user1" },
                new GetCommentResponse { Id = 2, Text = "Not bad", Created = DateTime.UtcNow, ProductId = 2, UserId = "user2" }
            };
            _mockCommentService.Setup(s => s.GetAllCommentsAsync())
                .ReturnsAsync(mockComments);

            // Act
            var result = await _controller.GetAllComments();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var comments = okResult.Value.Should().BeAssignableTo<IEnumerable<GetCommentResponse>>().Subject;
            comments.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetCommentById_ShouldReturnOkResult_WithComment()
        {
            // Arrange
            var mockComment = new GetCommentResponse { Id = 1, Text = "Great product!", Created = DateTime.UtcNow, ProductId = 1, UserId = "user1" };
            _mockCommentService.Setup(s => s.GetCommentByIdAsync(1))
                .ReturnsAsync(mockComment);

            // Act
            var result = await _controller.GetCommentById(1);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var comment = okResult.Value.Should().BeAssignableTo<GetCommentResponse>().Subject;
            comment.Id.Should().Be(1);
            comment.Text.Should().Be("Great product!");
        }

        [Fact]
        public async Task GetCommentById_ShouldReturnNotFound_WhenCommentDoesNotExist()
        {
            // Arrange
            _mockCommentService.Setup(s => s.GetCommentByIdAsync(1))
                .ReturnsAsync((GetCommentResponse)null);

            // Act
            var result = await _controller.GetCommentById(1);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task AddComment_ShouldReturnCreatedAtActionResult()
        {
            // Arrange
            var createCommentRequest = new CreateCommentRequest 
            { 
                Text = "Great product!", 
                ProductId = 1, 
                UserId = "user1" 
            };
            _mockCommentService.Setup(s => s.AddCommentAsync(createCommentRequest))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.AddComment(createCommentRequest);

            // Assert
            var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.ActionName.Should().Be(nameof(_controller.GetCommentById));
            createdResult.RouteValues["id"].Should().Be(1);
        }

        [Fact]
        public async Task UpdateComment_ShouldReturnNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var updateCommentRequest = new UpdateCommentRequest 
            { 
                Id = 1, 
                Text = "Updated comment", 
                ProductId = 1, 
                UserId = "user1" 
            };
            _mockCommentService.Setup(s => s.UpdateCommentAsync(updateCommentRequest))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateComment(1, updateCommentRequest);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdateComment_ShouldReturnNotFound_WhenCommentDoesNotExist()
        {
            // Arrange
            var updateCommentRequest = new UpdateCommentRequest 
            { 
                Id = 1, 
                Text = "Updated comment", 
                ProductId = 1, 
                UserId = "user1" 
            };
            _mockCommentService.Setup(s => s.UpdateCommentAsync(updateCommentRequest))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateComment(1, updateCommentRequest);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateComment_ShouldReturnBadRequest_WhenIdMismatch()
        {
            // Arrange
            var updateCommentRequest = new UpdateCommentRequest 
            { 
                Id = 2, 
                Text = "Updated comment", 
                ProductId = 1, 
                UserId = "user1" 
            };

            // Act
            var result = await _controller.UpdateComment(1, updateCommentRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be("comment ID mismatch");
        }

        [Fact]
        public async Task DeleteComment_ShouldReturnNoContent_WhenDeleteIsSuccessful()
        {
            // Arrange
            _mockCommentService.Setup(s => s.DeleteCommentAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteComment(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteComment_ShouldReturnNotFound_WhenCommentDoesNotExist()
        {
            // Arrange
            _mockCommentService.Setup(s => s.DeleteCommentAsync(1))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteComment(1);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
