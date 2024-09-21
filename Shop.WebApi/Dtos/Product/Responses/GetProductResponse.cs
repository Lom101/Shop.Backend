using Shop.WebAPI.Dtos.Brand;
using Shop.WebAPI.Dtos.Category;

namespace Shop.WebAPI.Dtos.Product.Responses;

public class GetProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string ImageUrl { get; set; }
    public int CategoryId { get; set; }
    public CategoryDto Category { get; set; }

    // Новые свойства
    public int Size { get; set; } // Размер
    public string Color { get; set; } // Цвет
    public int BrandId { get; set; }
    public BrandDto Brand { get; set; }  // Добавляем бренд в ответ
    public string Material { get; set; } // Материал
    public bool IsAvailable { get; set; } // Наличие
}
