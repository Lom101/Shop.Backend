using Shop.WebAPI.Dtos.Category;
using Shop.WebAPI.Dtos.Comment;

namespace Shop.WebAPI.Dtos.Product;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string ImageUrl { get; set; }
    public int CategoryId { get; set; }
    public CategoryDto Category { get; set; }

    public IEnumerable<CommentDto> Comments { get; set; }

    // Новые свойства
    public int[] Sizes { get; set; } // Массив размеров
    public string Color { get; set; } // Цвет
    public int BrandId { get; set; }  // Внешний ключ
    public string Brand { get; set; } // Бренд
    public string Material { get; set; } // Материал
    public bool IsAvailable { get; set; } // Наличие
}