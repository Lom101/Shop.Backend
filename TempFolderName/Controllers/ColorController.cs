using Microsoft.AspNetCore.Mvc;
using Shop.WebAPI.Repository.Interfaces;
using System.Threading.Tasks;

namespace Shop.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ColorController : ControllerBase
{
    private readonly IColorRepository _colorRepository;

    public ColorController(IColorRepository colorRepository)
    {
        _colorRepository = colorRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllColors()
    {
        var colors = await _colorRepository.GetAllAsync();
        return Ok(colors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetColorById(int id)
    {
        var color = await _colorRepository.GetByIdAsync(id);
        if (color == null)
        {
            return NotFound();
        }

        return Ok(color);
    }
}