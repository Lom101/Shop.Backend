using Shop.WebAPI.Entities;

namespace Shop.WebAPI.Repository.Interfaces;

public interface IProductRepository
{
    Task<Product> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetAllAsync();
    
    // Task<decimal> GetMinPriceAsync();
    // Task<decimal> GetMaxPriceAsync();
    // Task<List<string>> GetAvailableCategoriesAsync();
    // Task<List<string>> GetAvailableBrandsAsync();
    // Task<List<int>> GetAvailableSizesAsync();
    // Task<List<string>> GetAvailableColorsAsync();
    
    
    
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
    // Task<int> GetTotalCountProductsInCategory(int? categoryId);
}
