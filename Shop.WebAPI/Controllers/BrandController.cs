using Microsoft.AspNetCore.Mvc;
using Shop.WebAPI.Dtos.Brand.Request;
using Shop.WebAPI.Dtos.Brand.Response;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Shop.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrandController : ControllerBase
{
    private readonly IBrandRepository _brandRepository;
    private readonly IMapper _mapper;

    public BrandController(IBrandRepository brandRepository, IMapper mapper)
    {
        _brandRepository = brandRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBrands()
    {
        var brands = await _brandRepository.GetAllAsync();
        var response = _mapper.Map<IEnumerable<GetBrandResponse>>(brands);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBrandById(int id)
    {
        var brand = await _brandRepository.GetByIdAsync(id);
        if (brand == null)
            return NotFound($"Бренд с ID {id} не найден.");

        var response = _mapper.Map<GetBrandResponse>(brand);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateBrand(CreateBrandRequest request)
    {
        var brand = _mapper.Map<Brand>(request);
        var success = await _brandRepository.AddAsync(brand);
        if (!success)
            return BadRequest("Не удалось создать бренд.");

        var response = _mapper.Map<GetBrandResponse>(brand);
        return CreatedAtAction(nameof(GetBrandById), new { id = brand.Id }, response);
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateBrand(int id, UpdateBrandRequest request)
    {
        if (id != request.Id)
            return BadRequest("Несоответствие ID бренда.");

        var brand = await _brandRepository.GetByIdAsync(id);
        if (brand == null)
            return NotFound($"Бренд с ID {id} не найден.");

        _mapper.Map(request, brand);
        var success = await _brandRepository.UpdateAsync(brand);
        if (!success)
            return BadRequest("Не удалось обновить бренд.");
        
        var response = _mapper.Map<GetBrandResponse>(brand);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteBrand(int id)
    {
        var brand = await _brandRepository.GetByIdAsync(id);
        if (brand == null)
            return NotFound($"Бренд с ID {id} не найден.");
        
        var success = await _brandRepository.DeleteAsync(id);
        if (!success)
            return BadRequest("Не удалось удалить бренд.");

        return NoContent();
    }
}