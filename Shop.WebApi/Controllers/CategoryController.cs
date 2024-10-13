using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.WebAPI.Dtos.Category.Requests;
using Shop.WebAPI.Dtos.Category.Responses;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;
using AutoMapper;

namespace Shop.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        // GET: api/category
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var response = _mapper.Map<IEnumerable<GetCategoryResponse>>(categories);
            return Ok(response);
        }

        // GET: api/category/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return NotFound($"Категория с ID {id} не найдена.");

            var response = _mapper.Map<GetCategoryResponse>(category);
            return Ok(response);
        }

        // POST: api/category
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategory([FromBody] CreateCategoryRequest createCategoryRequest)
        {
            var existingCategory = await _categoryRepository.GetByNameAsync(createCategoryRequest.Name);
            if (existingCategory != null)
            {
                return BadRequest($"Категория с именем '{createCategoryRequest.Name}' уже существует.");
            }

            var newCategory = _mapper.Map<Category>(createCategoryRequest);
    
            // Attempt to add the new category and save changes
            var isAdded = await _categoryRepository.AddAsync(newCategory);
            if (!isAdded)
            {
                return BadRequest("Не удалось создать категорию.");
            }

            // The ID should be set automatically if using an ORM
            var response = _mapper.Map<GetCategoryResponse>(newCategory);
            return CreatedAtAction(nameof(GetCategoryById), new { id = newCategory.Id }, response);
        }



        // PUT: api/category/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryRequest updateCategoryRequest)
        {
            if (id != updateCategoryRequest.Id)
            {
                return BadRequest("Несоответствие ID категории.");
            }

            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound($"Категория с ID {id} не найдена.");
            }

            _mapper.Map(updateCategoryRequest, category);
            var isUpdated = await _categoryRepository.UpdateAsync(category);
            if (!isUpdated)
            {
                return BadRequest("Не удалось обновить категорию.");
            }

            var response = _mapper.Map<GetCategoryResponse>(category);
            return Ok(response);
        }

        // DELETE: api/category/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound($"Категория с ID {id} не найдена.");
            }

            var isDeleted = await _categoryRepository.DeleteAsync(id);
            if (!isDeleted)
            {
                return BadRequest("Не удалось удалить категорию.");
            }
            return NoContent();
        }
    }
}
