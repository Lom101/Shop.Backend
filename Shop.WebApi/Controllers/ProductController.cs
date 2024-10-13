using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shop.WebAPI.Dtos.Product.Requests;
using Shop.WebAPI.Dtos.Product.Responses;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;

namespace Shop.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductController(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    // GET: api/products
    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productRepository.GetAllAsync();
        var productDtos = _mapper.Map<IEnumerable<GetProductResponse>>(products);
        return Ok(productDtos);
    }

    // эндпоинт который возвращает элементы с учетом пагинации и фильтрации
    // GET: api/products/filteredPagedProducts
    [HttpGet("filtered_paged")]
    public async Task<IActionResult> GetFilteredPagedProducts(
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
        var result = await _productRepository.GetFilteredProductsAsync(
            categoryId, brandId, minPrice, maxPrice, inStock, sizeIds, colorIds);

        // Пагинация
        var totalCount = result.Count();
        var filteredPagedProducts = result
            .Skip((int)((pageNumber - 1) * pageSize))
            .Take((int)pageSize)
            .ToList();

        return Ok(new { Items = filteredPagedProducts, TotalCount = totalCount });
    }

    // Возвращаем все необходимые опции продукта (размеры, цвета и т.д.)
    // GET: api/products/options
    [HttpGet("filter_options")]
    public async Task<IActionResult> GetProductFilterOptionsAsync()
    {
        var categories = await _productRepository.GetAvailableCategoriesAsync();
        var brands = await _productRepository.GetAvailableBrandsAsync();
        var sizes = await _productRepository.GetAvailableSizesAsync();
        var colors = await _productRepository.GetAvailableColorsAsync();
        var minPrice = await _productRepository.GetMinPriceAsync();
        var maxPrice = await _productRepository.GetMaxPriceAsync();

        return Ok(new
        {
            Categories = categories,
            Brands = brands,
            Sizes = sizes,
            Colors = colors,
            MinPrice = minPrice,
            MaxPrice = maxPrice
        });
    }

    // GET: api/products/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        var productDto = _mapper.Map<GetProductResponse>(product); // Используем AutoMapper
        return Ok(productDto);
    }

    // POST: api/products
    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] CreateProductRequest createProductRequest)
    {
        var newProduct = _mapper.Map<Product>(createProductRequest);
        await _productRepository.AddAsync(newProduct);
        
        var productDto = _mapper.Map<GetProductResponse>(newProduct); // Используем AutoMapper
        return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, productDto);
    }

    // PUT: api/products/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductRequest updateProductRequest)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null)
        {
            return NotFound();
        }

        _mapper.Map(updateProductRequest, existingProduct); 
        await _productRepository.UpdateAsync(existingProduct);
        return NoContent();
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null)
        {
            return NotFound();
        }
        await _productRepository.DeleteAsync(id);
        return NoContent();
    }
}
