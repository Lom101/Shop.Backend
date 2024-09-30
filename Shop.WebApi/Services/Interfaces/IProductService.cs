using Microsoft.AspNetCore.Mvc;
using Shop.WebAPI.Dtos.Product;
using Shop.WebAPI.Dtos.Product.Requests;
using Shop.WebAPI.Dtos.Product.Responses;

namespace Shop.WebAPI.Services.Interfaces;

public interface IProductService
{
    Task<GetProductResponse> GetProductByIdAsync(int id);
    Task<IEnumerable<GetProductResponse>> GetAllProductsAsync();
    Task<GetProductFilterOptionsResponse> GetProductFilterOptionsAsync();
    Task<GetProductFilteredPagedResponse> GetFilteredPagedProductsAsync(
        int pageNumber,
        int pageSize,
        int? categoryId,
        int? brandId,
        double? minPrice,
        double? maxPrice,
        bool? inStock,
        [FromQuery] List<int> sizeIds,
        [FromQuery] List<int> colorIds);
    Task<int> AddProductAsync(CreateProductRequest productDto);
    Task<bool> UpdateProductAsync(UpdateProductRequest productDto);
    Task<bool> DeleteProductAsync(int id);
}
