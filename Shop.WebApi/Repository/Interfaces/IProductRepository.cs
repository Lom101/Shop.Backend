using Microsoft.AspNetCore.Mvc;
using Shop.WebAPI.Dtos.Brand.Response;
using Shop.WebAPI.Dtos.Category.Responses;
using Shop.WebAPI.Dtos.Color.Responses;
using Shop.WebAPI.Dtos.Product.Requests;
using Shop.WebAPI.Dtos.Size.Responses;
using Shop.WebAPI.Entities;

namespace Shop.WebAPI.Repository.Interfaces;

public interface IProductRepository
{
    Task<Product> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<IEnumerable<Product>> GetFilteredProductsAsync(
        int? categoryId,
        int? brandId,
        double? minPrice,
        double? maxPrice,
        bool? inStock,
        [FromQuery] List<int> sizeIds,
        [FromQuery] List<int> colorIds);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
    Task<IEnumerable<Category>> GetAvailableCategoriesAsync();
    Task<IEnumerable<Brand>> GetAvailableBrandsAsync();
    Task<IEnumerable<Size>> GetAvailableSizesAsync();
    Task<IEnumerable<Color>> GetAvailableColorsAsync();
    Task<decimal> GetMinPriceAsync();
    Task<decimal> GetMaxPriceAsync();
}
