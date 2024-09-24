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
    
    // public async Task<decimal> GetMinPriceAsync()
    // {
    //     return await _context.Products
    //         .MinAsync(p => p.Price);
    // }
    //
    // public async Task<decimal> GetMaxPriceAsync()
    // {
    //     return await _context.Products
    //         .MaxAsync(p => p.Price); // Получаем максимальную цену из товаров
    // }
    // public async Task<List<string>> GetAvailableCategoriesAsync()
    // {
    //     return await _context.Categories
    //         .Select(c => c.Name)
    //         .Distinct()
    //         .ToListAsync(); 
    // }
    //
    // public async Task<List<string>> GetAvailableBrandsAsync()
    // {
    //     return await _context.Brands
    //         .Select(b => b.Name)
    //         .Distinct()
    //         .ToListAsync();
    // }
    //
    // public async Task<List<int>> GetAvailableSizesAsync()
    // {
    //     // Сначала загружаем все продукты асинхронно
    //     var products = await _context.Products
    //         .ToListAsync();  // Асинхронный запрос к базе данных для загрузки всех продуктов
    //
    //     // Выполняем обработку массива размеров в памяти
    //     return products
    //         .SelectMany(p => p.Sizes)  // Получаем все размеры из каждого продукта
    //         .Distinct()  // Убираем дубликаты
    //         .ToList();  // Преобразуем результат в список
    // }
    //
    // public async Task<List<string>> GetAvailableColorsAsync()
    // {
    //     return await _context.Products
    //         .Select(p => p.Color)
    //         .Distinct()
    //         .ToListAsync();
    // }
    
    // public async Task<IQueryable<Product>> GetFilteredProductsAsync(
    //     int? categoryId, 
    //     int? brandId, 
    //     int? size, 
    //     string color, 
    //     decimal? minPrice, 
    //     decimal? maxPrice, 
    //     bool? inStock)
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
    //     // // Фильтрация по размеру ////////////////////////// ???????? 
    //     // if (size.HasValue)
    //     // {
    //     //     query = query.Where(p => p.Sizes == size.Value);
    //     // }
    //     
    //     // Фильтрация по цвету
    //     if (!string.IsNullOrEmpty(color))
    //     {
    //         query = query.Where(p => p.Color == color);
    //     }
    //
    //     // Фильтрация по цене
    //     if (minPrice.HasValue)
    //     {
    //         query = query.Where(p => p.Price >= minPrice.Value);
    //     }
    //     if (maxPrice.HasValue)
    //     {
    //         query = query.Where(p => p.Price <= maxPrice.Value);
    //     }
    //
    //     // Фильтрация по наличию на складе
    //     if (inStock.HasValue)
    //     {
    //         query = query.Where(p => p.StockQuantity > 0 == inStock.Value);
    //     }
    //
    //     return await Task.FromResult(query);
    // }

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
}

