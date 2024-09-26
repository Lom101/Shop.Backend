using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shop.WebAPI.Controllers;
using Shop.WebAPI.Dtos.Photo.Requests;
using Shop.WebAPI.Dtos.Photo.Responses;
using Shop.WebAPI.Services.Interfaces;

namespace Shop.Tests;

public class PhotoControllerTests
{
    private readonly Mock<IPhotoService> _mockPhotoService;
    private readonly PhotoController _photoController;

    public PhotoControllerTests()
    {
        _mockPhotoService = new Mock<IPhotoService>();
        _photoController = new PhotoController(_mockPhotoService.Object);
    }

    [Fact]
    public async Task UploadPhoto_ReturnsOkResult_WhenPhotoUploadedSuccessfully()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var content = "Fake file content";
        var fileName = "test.jpg";
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(content);
        writer.Flush();
        ms.Position = 0;

        fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
        fileMock.Setup(_ => _.FileName).Returns(fileName);
        fileMock.Setup(_ => _.Length).Returns(ms.Length);

        var request = new CreatePhotoRequest
        {
            ModelId = 1,
            File = fileMock.Object
        };

        var response = new GetPhotoResponse
        {
            Id = 1,
            FileName = fileName,
            Length = ms.Length,
            Url = "/images/test.jpg"
        };

        _mockPhotoService
            .Setup(x => x.UploadPhotoAsync(It.IsAny<CreatePhotoRequest>()))
            .ReturnsAsync(response);

        // Act
        var result = await _photoController.UploadPhoto(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);  // Ожидаем OkObjectResult
        var returnValue = Assert.IsType<GetPhotoResponse>(okResult.Value);
        Assert.Equal(response.Id, returnValue.Id);
        Assert.Equal(response.FileName, returnValue.FileName);
    }


    [Fact]
    public async Task UploadPhoto_ReturnsBadRequest_WhenFileIsEmpty()
    {
        // Arrange
        var request = new CreatePhotoRequest
        {
            ModelId = 1,
            File = null
        };

        // Act
        var result = await _photoController.UploadPhoto(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("File is empty.", badRequestResult.Value);
    }
    
    [Fact]
    public async Task UpdatePhoto_ReturnsOkResult_WhenPhotoUpdatedSuccessfully()
    {
        // Arrange
        var request = new UpdatePhotoRequest
        {
            Id = 1,
            ModelId = 1,
            File = new FormFile(null, 0, 0, null, "updated.jpg")
        };

        var response = new GetPhotoResponse
        {
            Id = 1,
            FileName = "updated.jpg",
            Length = 2048,
            Url = "/images/updated.jpg"
        };

        _mockPhotoService
            .Setup(x => x.UpdatePhotoAsync(It.IsAny<UpdatePhotoRequest>()))
            .ReturnsAsync(response);

        // Act
        var result = await _photoController.UpdatePhoto(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<GetPhotoResponse>(okResult.Value);
        Assert.Equal(response.Id, returnValue.Id);
        Assert.Equal(response.FileName, returnValue.FileName);
    }

    [Fact]
    public async Task UpdatePhoto_ReturnsNotFound_WhenPhotoDoesNotExist()
    {
        // Arrange
        var request = new UpdatePhotoRequest
        {
            Id = 1,
            ModelId = 1,
            File = new FormFile(null, 0, 0, null, "updated.jpg")
        };

        _mockPhotoService
            .Setup(x => x.UpdatePhotoAsync(It.IsAny<UpdatePhotoRequest>()))
            .ReturnsAsync((GetPhotoResponse)null); // Возвращаем null, если фото не найдено

        // Act
        var result = await _photoController.UpdatePhoto(request);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Photo not found.", notFoundResult.Value);
    }
    [Fact]
    public async Task DeletePhoto_ReturnsNoContent_WhenPhotoDeletedSuccessfully()
    {
        // Arrange
        var photoId = 1;

        _mockPhotoService
            .Setup(x => x.DeletePhotoAsync(photoId))
            .ReturnsAsync(true);

        // Act
        var result = await _photoController.DeletePhoto(photoId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeletePhoto_ReturnsNotFound_WhenPhotoDoesNotExist()
    {
        // Arrange
        var photoId = 1;

        _mockPhotoService
            .Setup(x => x.DeletePhotoAsync(photoId))
            .ReturnsAsync(false); // Фотография не найдена

        // Act
        var result = await _photoController.DeletePhoto(photoId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Photo not found or could not be deleted.", notFoundResult.Value);
    }
}

