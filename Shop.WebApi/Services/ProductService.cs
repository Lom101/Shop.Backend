using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shop.WebAPI.Dtos.Brand.Response;
using Shop.WebAPI.Dtos.Category.Responses;
using Shop.WebAPI.Dtos.Color.Responses;
using Shop.WebAPI.Dtos.Product;
using Shop.WebAPI.Dtos.Product.Requests;
using Shop.WebAPI.Dtos.Product.Responses;
using Shop.WebAPI.Dtos.Size.Responses;
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
    public async Task<GetProductFilterOptionsResponse> GetProductFilterOptionsAsync()
    {
        var categories = await _productRepository.GetAvailableCategoriesAsync();
        var brands = await _productRepository.GetAvailableBrandsAsync();
        var sizes = await _productRepository.GetAvailableSizesAsync();
        var colors = await _productRepository.GetAvailableColorsAsync();
        var minPrice = await _productRepository.GetMinPriceAsync();
        var maxPrice = await _productRepository.GetMaxPriceAsync();
        
        return new GetProductFilterOptionsResponse()
        {
            Categories = _mapper.Map<IEnumerable<GetCategoryResponse>>(categories),
            Brands = _mapper.Map<IEnumerable<GetBrandResponse>>(brands),
            Sizes = _mapper.Map<IEnumerable<GetSizeResponse>>(sizes),
            Colors = _mapper.Map<IEnumerable<GetColorResponse>>(colors),
            MinPrice = minPrice, // Установи минимальную цену
            MaxPrice = maxPrice, // Максимальная цена
        };
    }

    
    
    public async Task<GetProductFilteredPagedResponse> GetFilteredPagedProductsAsync(
        int pageNumber,
        int pageSize,
        int? categoryId,
        int? brandId,
        double? minPrice,
        double? maxPrice,
        bool? inStock,
        [FromQuery] List<int> sizeIds,
        [FromQuery] List<int> colorIds)
    {
        // Получаем отфильтрованные данные
        var filteredProducts = await _productRepository.GetFilteredProductsAsync(
            categoryId, brandId, minPrice, maxPrice, inStock, sizeIds, colorIds);
        
        // Пагинация
        var totalCount = filteredProducts.Count();
        var filteredPagedProducts =  filteredProducts
            .Skip((int)((pageNumber - 1) * pageSize))
            .Take((int)pageSize)
            .ToList();
        
        return new GetProductFilteredPagedResponse()
        {
            Items = _mapper.Map<List<GetProductResponse>>(filteredPagedProducts),
            TotalCount = totalCount
        };
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

    // public async Task<int> GetTotalCountProductsInCategory(int? categoryId)
    // {
    //     return await _productRepository.GetTotalCountProductsInCategory(categoryId);
    // }
}
