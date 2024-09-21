using Shop.WebAPI.Entities;

namespace Shop.WebAPI.Repository.Interfaces;

public interface IProductRepository
{
    Task<Product> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<IQueryable<Product>> GetFilteredProductsAsync(
        int? categoryId, 
        int? brandId, 
        int? size, 
        string color, 
        decimal? minPrice, 
        decimal? maxPrice, 
        bool? inStock);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
    // Task<int> GetTotalCountProductsInCategory(int? categoryId);
}
