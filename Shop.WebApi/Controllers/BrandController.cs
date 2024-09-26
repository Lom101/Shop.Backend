using Microsoft.AspNetCore.Mvc;
using Shop.WebAPI.Dtos.Brand.Request;
using Shop.WebAPI.Services.Interfaces;

namespace Shop.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrandController : ControllerBase
{
    private readonly IBrandService _brandService;

    public BrandController(IBrandService brandService)
    {
        _brandService = brandService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBrands()
    {
        var brands = await _brandService.GetAllBrandsAsync();
        return Ok(brands); // Возвращаем BrandDto
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBrandById(int id)
    {
        var brand = await _brandService.GetBrandByIdAsync(id);
        if (brand == null)
            return NotFound();
        return Ok(brand);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBrand(CreateBrandRequest request)
    {
        var brand = await _brandService.AddBrandAsync(request);
        return CreatedAtAction(nameof(GetAllBrands), new { id = brand.Id }, brand); // Возвращаем BrandDto
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBrand(int id, UpdateBrandRequest request)
    {
        if (id != request.Id)
            return BadRequest("brand ID mismatch");

        var updatedBrand = await _brandService.UpdateBrandAsync(request);
        if (updatedBrand == null)
            return NotFound();

        return Ok(updatedBrand); // Возвращаем обновленный BrandDto
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBrand(int id)
    {
        var success = await _brandService.DeleteBrandAsync(id);
        if (!success)
            return NotFound();

        return NoContent();
    }
}
