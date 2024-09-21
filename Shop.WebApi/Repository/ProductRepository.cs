using Microsoft.EntityFrameworkCore;
using Shop.WebAPI.Data;
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
        return await _context.Products.FindAsync(id);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }
    
    public async Task<IQueryable<Product>> GetFilteredProductsAsync(
        int? categoryId, 
        int? brandId, 
        int? size, 
        string color, 
        decimal? minPrice, 
        decimal? maxPrice, 
        bool? inStock)
    {
        var query = _context.Products.AsQueryable();

        // Фильтрация по категории
        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        // Фильтрация по бренду
        if (brandId.HasValue)
        {
            query = query.Where(p => p.BrandId == brandId.Value);
        }

        // // Фильтрация по размеру ////////////////////////// ???????? 
        // if (size.HasValue)
        // {
        //     query = query.Where(p => p.Size == size.Value);
        // }
        
        // Фильтрация по цвету
        if (!string.IsNullOrEmpty(color))
        {
            query = query.Where(p => p.Color == color);
        }

        // Фильтрация по цене
        if (minPrice.HasValue)
        {
            query = query.Where(p => p.Price >= minPrice.Value);
        }
        if (maxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= maxPrice.Value);
        }

        // Фильтрация по наличию на складе
        if (inStock.HasValue)
        {
            query = query.Where(p => p.StockQuantity > 0 == inStock.Value);
        }

        return await Task.FromResult(query);
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

    // public async Task<int> GetTotalCountProductsInCategory(int? categoryId)
    // {
    //     if (categoryId.HasValue)
    //     {
    //         var arr = await _context.Products
    //             .Where(p => p.CategoryId == categoryId)
    //             .ToListAsync();
    //         return arr.Count;
    //     }
    //     // если категорию не передали, то возвращаем общее количество товаров
    //     else
    //     {
    //         var arr = await _context.Products
    //             .ToListAsync();
    //         return arr.Count;
    //     }
    // }
}

