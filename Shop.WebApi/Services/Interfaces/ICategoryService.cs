using Shop.WebAPI.Dtos.Category.Requests;
using Shop.WebAPI.Dtos.Category.Responses;

namespace Shop.WebAPI.Services.Interfaces;

public interface ICategoryService
{
    Task<GetCategoryResponse> GetCategoryByIdAsync(int id);
    Task<IEnumerable<GetCategoryResponse>> GetAllCategoriesAsync();
    Task<int> AddCategoryAsync(CreateCategoryRequest categoryDto);
    Task<bool> UpdateCategoryAsync(UpdateCategoryRequest categoryDto);
    Task<bool> DeleteCategoryAsync(int id);
}