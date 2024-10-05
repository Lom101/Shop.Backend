// using System.Collections.Generic;
// using System.Threading.Tasks;
// using FluentAssertions;
// using Microsoft.AspNetCore.Mvc;
// using Moq;
// using Shop.WebAPI.Controllers;
// using Shop.WebAPI.Dtos.Address.Requests;
// using Shop.WebAPI.Dtos.Address.Responses;
// using Shop.WebAPI.Services.Interfaces;
// using Xunit;
//
// namespace Shop.Tests
// {
//     public class AddressControllerTests
//     {
//         private readonly Mock<IAddressService> _mockAddressService;
//         private readonly AddressController _controller;
//
//         public AddressControllerTests()
//         {
//             _mockAddressService = new Mock<IAddressService>();
//             _controller = new AddressController(_mockAddressService.Object);
//         }
//
//         [Fact]
//         public async Task GetAllAddresses_ShouldReturnOkResult_WithListOfAddresses()
//         {
//             // Arrange
//             var mockAddresses = new List<GetAddressResponse>
//             {
//                 new GetAddressResponse { Id = 1, Street = "Street 1" },
//                 new GetAddressResponse { Id = 2, Street = "Street 2" }
//             };
//             _mockAddressService.Setup(s => s.GetAllAddressesAsync())
//                 .ReturnsAsync(mockAddresses);
//
//             // Act
//             var result = await _controller.GetAllAddresses();
//
//             // Assert
//             var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
//             var addresses = okResult.Value.Should().BeAssignableTo<IEnumerable<GetAddressResponse>>().Subject;
//             addresses.Should().HaveCount(2);
//         }
//
//         [Fact]
//         public async Task GetAddressById_ShouldReturnOkResult_WithAddress()
//         {
//             // Arrange
//             var mockAddress = new GetAddressResponse { Id = 1, Street = "Street 1" };
//             _mockAddressService.Setup(s => s.GetAddressByIdAsync(1))
//                 .ReturnsAsync(mockAddress);
//
//             // Act
//             var result = await _controller.GetAddressById(1);
//
//             // Assert
//             var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
//             var address = okResult.Value.Should().BeAssignableTo<GetAddressResponse>().Subject;
//             address.Id.Should().Be(1);
//             address.Street.Should().Be("Street 1");
//         }
//
//         [Fact]
//         public async Task GetAddressById_ShouldReturnNotFound_WhenAddressDoesNotExist()
//         {
//             // Arrange
//             _mockAddressService.Setup(s => s.GetAddressByIdAsync(1))
//                 .ReturnsAsync((GetAddressResponse)null);
//
//             // Act
//             var result = await _controller.GetAddressById(1);
//
//             // Assert
//             result.Should().BeOfType<NotFoundResult>();
//         }
//
//         [Fact]
//         public async Task AddAddress_ShouldReturnCreatedAtActionResult()
//         {
//             // Arrange
//             var createAddressRequest = new CreateAddressRequest { Street = "Street 1" };
//             _mockAddressService.Setup(s => s.AddAddressAsync(createAddressRequest))
//                 .ReturnsAsync(1);
//
//             // Act
//             var result = await _controller.AddAddress(createAddressRequest);
//
//             // Assert
//             var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
//             createdResult.ActionName.Should().Be(nameof(_controller.GetAddressById));
//             createdResult.RouteValues["id"].Should().Be(1);
//         }
//
//         [Fact]
//         public async Task UpdateAddress_ShouldReturnNoContent_WhenUpdateIsSuccessful()
//         {
//             // Arrange
//             var updateAddressRequest = new UpdateAddressRequest { Id = 1, Street = "Updated Street" };
//             _mockAddressService.Setup(s => s.UpdateAddressAsync(updateAddressRequest))
//                 .ReturnsAsync(true);
//
//             // Act
//             var result = await _controller.UpdateAddress(1, updateAddressRequest);
//
//             // Assert
//             result.Should().BeOfType<NoContentResult>();
//         }
//
//         [Fact]
//         public async Task UpdateAddress_ShouldReturnNotFound_WhenAddressDoesNotExist()
//         {
//             // Arrange
//             var updateAddressRequest = new UpdateAddressRequest { Id = 1, Street = "Updated Street" };
//             _mockAddressService.Setup(s => s.UpdateAddressAsync(updateAddressRequest))
//                 .ReturnsAsync(false);
//
//             // Act
//             var result = await _controller.UpdateAddress(1, updateAddressRequest);
//
//             // Assert
//             result.Should().BeOfType<NotFoundResult>();
//         }
//
//         [Fact]
//         public async Task UpdateAddress_ShouldReturnBadRequest_WhenIdMismatch()
//         {
//             // Arrange
//             var updateAddressRequest = new UpdateAddressRequest { Id = 2, Street = "Updated Street" };
//
//             // Act
//             var result = await _controller.UpdateAddress(1, updateAddressRequest);
//
//             // Assert
//             result.Should().BeOfType<BadRequestObjectResult>()
//                 .Which.Value.Should().Be("address ID mismatch");
//         }
//
//         [Fact]
//         public async Task DeleteAddress_ShouldReturnNoContent_WhenDeleteIsSuccessful()
//         {
//             // Arrange
//             _mockAddressService.Setup(s => s.DeleteAddressAsync(1))
//                 .ReturnsAsync(true);
//
//             // Act
//             var result = await _controller.DeleteAddress(1);
//
//             // Assert
//             result.Should().BeOfType<NoContentResult>();
//         }
//
//         [Fact]
//         public async Task DeleteAddress_ShouldReturnNotFound_WhenAddressDoesNotExist()
//         {
//             // Arrange
//             _mockAddressService.Setup(s => s.DeleteAddressAsync(1))
//                 .ReturnsAsync(false);
//
//             // Act
//             var result = await _controller.DeleteAddress(1);
//
//             // Assert
//             result.Should().BeOfType<NotFoundResult>();
//         }
//     }
// }
