using Shop.WebAPI.Dtos.Product;
using Shop.WebAPI.Dtos.Product.Requests;
using Shop.WebAPI.Dtos.Product.Responses;

namespace Shop.WebAPI.Services.Interfaces;

public interface IProductService
{
    Task<GetProductResponse> GetProductByIdAsync(int id);
    Task<IEnumerable<GetProductResponse>> GetAllProductsAsync();
    // Task<ProductOptionsDto> GetProductOptionsAsync();
    // Task<FilteredPagedProductResponse> GetFilteredPagedProductsAsync(
    //     int pageNumber, 
    //     int pageSize, 
    //     int? categoryId, 
    //     int? brandId, 
    //     int? size, 
    //     string color, 
    //     decimal? minPrice, 
    //     decimal? maxPrice, 
    //     bool? inStock);
    Task<int> AddProductAsync(CreateProductRequest productDto);
    Task<bool> UpdateProductAsync(UpdateProductRequest productDto);
    Task<bool> DeleteProductAsync(int id);
}
