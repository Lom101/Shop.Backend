using System.Collections.ObjectModel;

namespace Shop.WebAPI.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Created { get; set; }
    
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    
    public int BrandId { get; set; }
    public Brand Brand { get; set; }
    
    public ICollection<Model> Models { get; set; }
    
    // public Product()
    // {
    //     Created = DateTime.Now;
    //     Models = new Collection<Model>();
    // }
}



// public ICollection<Comment> Comments { get; set; }
//
// // Новые свойства для кроссовок
// public int[] Sizes { get; set; } // все размеры товара
// public string Color { get; set; } // Цвет
// // Бренд
    
// public string Material { get; set; } // Материал
// public bool IsAvailable { get; set; } // Наличие на складе
