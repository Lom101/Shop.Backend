using AutoMapper;
using Shop.WebAPI.Dtos.Category.Requests;
using Shop.WebAPI.Dtos.Category.Responses;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;
using Shop.WebAPI.Services.Interfaces;

namespace Shop.WebAPI.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper; // AutoMapper

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<GetCategoryResponse> GetCategoryByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        return _mapper.Map<GetCategoryResponse>(category);
    }

    public async Task<IEnumerable<GetCategoryResponse>> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<GetCategoryResponse>>(categories);
    }

    public async Task<int> AddCategoryAsync(CreateCategoryRequest categoryDto)
    {
        var category = _mapper.Map<Category>(categoryDto);
        await _categoryRepository.AddAsync(category);
        return category.Id;
    }

    public async Task<bool> UpdateCategoryAsync(UpdateCategoryRequest categoryDto)
    {
        var category = _mapper.Map<Category>(categoryDto);
        var existingCategory = await _categoryRepository.GetByIdAsync(category.Id);
        if (existingCategory == null)
        {
            return false; // Продукт не найден
        }
        await _categoryRepository.UpdateAsync(category);
        return true;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var existingCategory = await _categoryRepository.GetByIdAsync(id);
        if (existingCategory == null)
        {
            return false; // Продукт не найден
        }
        await _categoryRepository.DeleteAsync(id);
        return true;
    }
}
