using Microsoft.AspNetCore.Mvc;
using Shop.WebAPI.Services.Interfaces;

namespace Shop.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SizeController : ControllerBase
{
    private readonly ISizeService _sizeService;

    public SizeController(ISizeService sizeService)
    {
        _sizeService = sizeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSizes()
    {
        var sizes = await _sizeService.GetAllSizesAsync();
        return Ok(sizes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSizeById(int id)
    {
        var size = await _sizeService.GetSizeByIdAsync(id);
        if (size == null)
        {
            return NotFound();
        }

        return Ok(size);
    }
}
