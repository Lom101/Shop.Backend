using AutoMapper;
using Shop.WebAPI.Dtos.Product;
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

    public async Task<ProductOptionsDto> GetProductOptionsAsync()
    {
        var categories = await _productRepository.GetAvailableCategoriesAsync();
        var brands = await _productRepository.GetAvailableBrandsAsync();
        var sizes = await _productRepository.GetAvailableSizesAsync();
        var colors = await _productRepository.GetAvailableColorsAsync();
        var minPrice = await _productRepository.GetMinPriceAsync();
        var maxPrice = await _productRepository.GetMaxPriceAsync();
        
        return new ProductOptionsDto()
        {
            Categories = categories,
            Brands = brands,
            Sizes = sizes,
            Colors = colors,
            MinPrice = minPrice, // Установи минимальную цену
            MaxPrice = maxPrice, // Максимальная цена
            InStock = true // По умолчанию: товар в наличии
        };
    }

    public async Task<FilteredPagedProductResponse> GetFilteredPagedProductsAsync(
        int pageNumber, 
        int pageSize, 
        int? categoryId, 
        int? brandId, 
        int? size, 
        string color, 
        decimal? minPrice, 
        decimal? maxPrice, 
        bool? inStock)
    {
        // Получаем отфильтрованные данные
        var products = await _productRepository.GetFilteredProductsAsync(categoryId, brandId, size, color, minPrice, maxPrice, inStock);

        // Пагинация
        var totalItems = products.Count();
        var pagedProducts = products.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
    
        var productDtos = _mapper.Map<List<ProductDto>>(pagedProducts);
        
        return new FilteredPagedProductResponse
        {
            Products = productDtos,
            TotalItems = totalItems,
            PageNumber = pageNumber,
            PageSize = pageSize
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
