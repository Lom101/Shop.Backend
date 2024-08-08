using AutoMapper;
using Shop.WebAPI.Dtos.Product.Requests;
using Shop.WebAPI.Dtos.Product.Responses;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;
using Shop.WebAPI.Services.Interfaces;

namespace Shop.WebAPI.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper; // AutoMapper

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<GetProductResponse> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return _mapper.Map<GetProductResponse>(product);
    }

    public async Task<IEnumerable<GetProductResponse>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<GetProductResponse>>(products);
    }

    public async Task<int> AddProductAsync(CreateProductRequest productDto)
    {
        var product = _mapper.Map<Product>(productDto);
        await _productRepository.AddAsync(product);
        return product.Id;
    }

    public async Task<bool> UpdateProductAsync(UpdateProductRequest productDto)
    {
        var product = _mapper.Map<Product>(productDto);
        var existingProduct = await _productRepository.GetByIdAsync(product.Id);
        if (existingProduct == null)
        {
            return false; // Продукт не найден
        }
        await _productRepository.UpdateAsync(product);
        return true;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null)
        {
            return false; // Продукт не найден
        }
        await _productRepository.DeleteAsync(id);
        return true;
    }
}
