using Microsoft.AspNetCore.Mvc;
using Shop.WebAPI.Services.Interfaces;

namespace Shop.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ColorController : ControllerBase
{
    private readonly IColorService _colorService;

    public ColorController(IColorService colorService)
    {
        _colorService = colorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllColors()
    {
        var colors = await _colorService.GetAllColorsAsync();
        return Ok(colors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetColorById(int id)
    {
        var color = await _colorService.GetColorByIdAsync(id);
        if (color == null)
        {
            return NotFound();
        }

        return Ok(color);
    }
}
