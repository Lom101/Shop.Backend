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
    
    // Возвращаем все необходимые опции продукта (размеры, цвета и т.д.)
    // GET: api/products/options
    [HttpGet("options")]
    public async Task<IActionResult> GetProductOptionsAsync()
    {
        var productOptions = await _productService.GetProductOptionsAsync();
        return Ok(productOptions);
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