using Microsoft.AspNetCore.Mvc;
using Shop.WebAPI.Dtos.Product.Requests;
using Shop.WebAPI.Services.Interfaces;

namespace Shop.WebAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    // GET: api/products
    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }
    
    // эндпоинт который возвращает элементы с учетом пагинации (на определенную страницу и определенное количество)
    // GET: api/products/filteredPagedProducts
    [HttpGet("filteredPagedProducts")]
    public async Task<IActionResult> GetFilteredPagedProducts(
        int pageNumber = 1, 
        int pageSize = 10, 
        int? categoryId = null, 
        int? brandId = null, 
        int? size = null,
        string color = null,
        decimal? minPrice = null, 
        decimal? maxPrice = null,
        bool? inStock = null)
    {
        var result = await _productService.GetFilteredPagedProductsAsync(pageNumber, pageSize, categoryId, brandId, size, color, minPrice, maxPrice, inStock);
        return Ok(result);
    }
    
    // // эндпоинт возвращает количество товаров у которых категория равна - categoryId
    // // GET: api/products/totalCountInCategory
    // [HttpGet("totalCountInCategory")]
    // public async Task<IActionResult> GetTotalCountProductsInCategory([FromQuery] int? categoryId)
    // {
    //     var products = await _productService.GetTotalCountProductsInCategory(categoryId);
    //     return Ok(products);
    // }

    // GET: api/products/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    // POST: api/products
    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] CreateProductRequest createProductRequest)
    {
        var newProductId = await _productService.AddProductAsync(createProductRequest);
        return CreatedAtAction(nameof(GetProductById), new { id = newProductId }, createProductRequest);
    }

    // PUT: api/products/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductRequest updateProductRequest)
    {
        if (id != updateProductRequest.Id)
        {
            return BadRequest("product ID mismatch");
        }

        var isUpdated = await _productService.UpdateProductAsync(updateProductRequest);
        if (!isUpdated)
        {
            return NotFound();
        }
        return NoContent();
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var isDeleted = await _productService.DeleteProductAsync(id);
        if (!isDeleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}
//
// [HttpGet("products")]
// public async Task<IActionResult> GetFilteredPagedProducts(
//     int pageNumber = 1, 
//     int pageSize = 10, 
//     int? categoryId = null, 
//     int? brandId = null, 
//     int? size = null)
// {
//     var query = _context.Products.AsQueryable();
//
//     // Фильтрация по категории
//     if (categoryId.HasValue)
//     {
//         query = query.Where(p => p.CategoryId == categoryId.Value);
//     }
//
//     // Фильтрация по бренду
//     if (brandId.HasValue)
//     {
//         query = query.Where(p => p.BrandId == brandId.Value);
//     }
//
//     // Фильтрация по размеру
//     if (size.HasValue)
//     {
//         query = query.Where(p => p.Sizes.Any(s => s == size.Value));
//     }
//
//     // Пагинация
//     var totalItems = await query.CountAsync();
//     var products = await query
//         .Skip((pageNumber - 1) * pageSize)
//         .Take(pageSize)
//         .ToListAsync();
//
//     // Формирование ответа
//     var response = new PagedProductResponse
//     {
//         Products = products,
//         TotalItems = totalItems,
//         PageNumber = pageNumber,
//         PageSize = pageSize
//     };
//
//     return Ok(response);
// }
