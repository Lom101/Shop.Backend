using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.WebAPI.Data;
using Shop.WebAPI.Dtos.Product.Requests;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;

namespace Shop.WebAPI.Repository;

public class ProductRepository : IProductRepository
{
    private readonly ShopApplicationContext _context;

    public ProductRepository(ShopApplicationContext context)
    {
        _context = context;
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Models).ThenInclude(m => m.Color)
            .Include(p => p.Models).ThenInclude(m => m.ModelSizes).ThenInclude(ms => ms.Size)
            .Include(p => p.Models).ThenInclude(m => m.Photos)
            .Include(p => p.Comments) // Загружаем связанные комментарии
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Models).ThenInclude(m => m.Color)
            .Include(p => p.Models).ThenInclude(m => m.ModelSizes).ThenInclude(ms => ms.Size)
            .Include(p => p.Models).ThenInclude(m => m.Photos)
            .Include(p => p.Comments) // Загружаем связанные комментарии
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetFilteredProductsAsync(
        int? categoryId,
        int? brandId,
        double? minPrice,
        double? maxPrice,
        bool? inStock,
        [FromQuery] List<int> sizeIds,
        [FromQuery] List<int> colorIds
    )
    {
        var query = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Models).ThenInclude(m => m.Color)
            .Include(p => p.Models).ThenInclude(m => m.ModelSizes).ThenInclude(ms => ms.Size)
            .Include(p => p.Models).ThenInclude(m => m.Photos)
            .Include(p => p.Comments)
            .AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (brandId.HasValue)
            query = query.Where(p => p.BrandId == brandId.Value);

        if (minPrice.HasValue)
            query = query.Where(p => p.Models.Any(m => m.Price >= minPrice.Value));

        if (maxPrice.HasValue)
            query = query.Where(p => p.Models.Any(m => m.Price <= maxPrice.Value));

        if (inStock.HasValue)
            query = query.Where(p => p.Models.Any(m =>
                m.ModelSizes.Any(ms => ms.StockQuantity > 0)) == inStock.Value);

        if (sizeIds != null && sizeIds.Any())
            query = query.Where(p => p.Models
                .SelectMany(m => m.ModelSizes)
                .Any(ms => sizeIds.Contains(ms.SizeId)));

        if (colorIds != null && colorIds.Any())
            query = query.Where(p => p.Models.Any(m => colorIds.Contains(m.ColorId)));

        return await query.ToListAsync();
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await GetByIdAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Category>> GetAvailableCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<IEnumerable<Brand>> GetAvailableBrandsAsync()
    {
        return await _context.Brands.ToListAsync();
    }

    public async Task<IEnumerable<Size>> GetAvailableSizesAsync()
    {
        return await _context.Sizes.ToListAsync();
    }

    public async Task<IEnumerable<Color>> GetAvailableColorsAsync()
    {
        return await _context.Colors.ToListAsync();
    }

    public async Task<decimal> GetMinPriceAsync()
    {
        return (decimal)await _context.Models.MinAsync(m => m.Price);
    }

    public async Task<decimal> GetMaxPriceAsync()
    {
        return (decimal)await _context.Models.MaxAsync(m => m.Price);
    }
}
