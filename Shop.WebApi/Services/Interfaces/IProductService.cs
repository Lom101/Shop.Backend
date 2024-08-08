using Shop.WebAPI.Dtos.Product.Requests;
using Shop.WebAPI.Dtos.Product.Responses;

namespace Shop.WebAPI.Services.Interfaces;

public interface IProductService
{
    Task<GetProductResponse> GetProductByIdAsync(int id);
    Task<IEnumerable<GetProductResponse>> GetAllProductsAsync();
    Task<int> AddProductAsync(CreateProductRequest productDto);
    Task<bool> UpdateProductAsync(UpdateProductRequest productDto);
    Task<bool> DeleteProductAsync(int id);
}
