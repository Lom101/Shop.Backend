using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.WebAPI.Dtos.Photo.Requests;
using Shop.WebAPI.Services.Interfaces;

namespace Shop.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PhotoController : ControllerBase
{
    private readonly IPhotoService _photoService;

    public PhotoController(IPhotoService photoService)
    {
        _photoService = photoService;
    }

    [HttpPost] 
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UploadPhoto([FromForm] CreatePhotoRequest request)
    {
        if (request.File == null || request.File.Length == 0)
        {
            return BadRequest("File is empty.");
        }

        var photoResponse = await _photoService.UploadPhotoAsync(request);

        if (photoResponse == null)
        {
            return BadRequest("Could not upload photo.");
        }

        return Ok(photoResponse);
    }


    [HttpPut("update")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdatePhoto([FromForm] UpdatePhotoRequest request)
    {
        var result = await _photoService.UpdatePhotoAsync(request);
        if (result == null)
        {
            return NotFound("Photo not found.");
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeletePhoto(int id)
    {
        var success = await _photoService.DeletePhotoAsync(id);
        if (!success)
        {
            return NotFound("Photo not found or could not be deleted.");
        }

        return NoContent();
    }
}
