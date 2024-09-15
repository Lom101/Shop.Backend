using Microsoft.AspNetCore.Mvc;
using Shop.WebAPI.Dtos.Category.Requests;
using Shop.WebAPI.Services.Interfaces;

namespace Shop.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET: api/category
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        return Ok(categories);
    }

    // GET: api/category/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return Ok(category);
    }

    // POST: api/category
    [HttpPost]
    public async Task<IActionResult> AddCategory([FromBody] CreateCategoryRequest createCategoryRequest)
    {
        var newCategoryId = await _categoryService.AddCategoryAsync(createCategoryRequest);
        return CreatedAtAction(nameof(GetCategoryById), new { id = newCategoryId }, createCategoryRequest);
    }

    // PUT: api/category/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryRequest updateCategoryRequest)
    {
        if (id != updateCategoryRequest.Id)
        {
            return BadRequest("category ID mismatch");
        }

        var isUpdated = await _categoryService.UpdateCategoryAsync(updateCategoryRequest);
        if (!isUpdated)
        {
            return NotFound();
        }
        return NoContent();
    }

    // DELETE: api/category/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var isDeleted = await _categoryService.DeleteCategoryAsync(id);
        if (!isDeleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}
