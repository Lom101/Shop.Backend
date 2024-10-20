using Microsoft.AspNetCore.Mvc;
using Shop.WebAPI.Repository.Interfaces;
namespace Shop.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SizeController : ControllerBase
{
    private readonly ISizeRepository _sizeRepository;

    public SizeController(ISizeRepository sizeRepository)
    {
        _sizeRepository = sizeRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSizes()
    {
        var sizes = await _sizeRepository.GetAllSizesAsync();
        return Ok(sizes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSizeById(int id)
    {
        var size = await _sizeRepository.GetSizeByIdAsync(id);
        if (size == null)
        {
            return NotFound();
        }

        return Ok(size);
    }
}