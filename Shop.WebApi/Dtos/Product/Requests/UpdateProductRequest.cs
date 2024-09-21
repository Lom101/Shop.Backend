namespace Shop.WebAPI.Dtos.Product.Requests;

public class UpdateProductRequest
{
    public int Id { get; set; } // Required for identifying which product to update
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string ImageUrl { get; set; }
    public int CategoryId { get; set; }

    // Новые свойства
    public int Size { get; set; } // Размер
    public string Color { get; set; } // Цвет
    public int BrandId { get; set; }  // Добавляем бренд

    public string Material { get; set; } // Материал
    public bool IsAvailable { get; set; } // Наличие
}