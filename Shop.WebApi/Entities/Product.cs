namespace Shop.WebAPI.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string ImageUrl { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public ICollection<Comment> Comments { get; set; }

    // Новые свойства для кроссовок
    public int Size { get; set; } // Размер
    public string Color { get; set; } // Цвет
    
    // Бренд
    public int BrandId { get; set; }  // Внешний ключ
    public Brand Brand { get; set; }  // Навигационное свойство
    public string Material { get; set; } // Материал
    public bool IsAvailable { get; set; } // Наличие на складе
}
